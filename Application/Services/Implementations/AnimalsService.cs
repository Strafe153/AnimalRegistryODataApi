using Application.DTOs.Animal;
using Application.Helpers.Interfaces;
using Application.Mappings;
using Application.Services.Interfaces;
using DataAccess.Extensions;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.Extensions.Logging;
using ValidationException = Application.Exceptions.ValidationException;

namespace Application.Services.Implementations;

public class AnimalsService : IAnimalsService
{
	private readonly IMapperSession<Animal> _animalSession;
	private readonly IMapperSession<Owner> _ownerSession;
	private readonly ITransactionRunner _transactionRunner;
	private readonly IValidator<AnimalUpdateDto> _validator;
	private readonly ILogger<AnimalsService> _logger;

	public AnimalsService(
		IMapperSession<Animal> animalSession,
		IMapperSession<Owner> ownerSession,
		ITransactionRunner transactionRunner,
		IValidator<AnimalUpdateDto> validator,
		ILogger<AnimalsService> logger)
	{
		_animalSession = animalSession;
		_ownerSession = ownerSession;
		_transactionRunner = transactionRunner;
		_validator = validator;
		_logger = logger;
	}

	public async Task<AnimalReadDto> CreateAsync(AnimalCreateDto dto)
	{
		var owner = _ownerSession.GetByIdOrThrow(dto.OwnerId, _logger);

		var animal = dto.ToAnimal();
		animal.Owner = owner;

		await _transactionRunner.RunInTransactionAsync(
			() => _animalSession.SaveAsync(animal),
			_animalSession,
			$"Failed to create an animal.");

		_logger.LogInformation("Successfully created an animal");

		return animal.ToReadDto();
	}

	public IQueryable<AnimalReadDto> GetAll()
	{
		var mappedQuery = _animalSession.GetAll().Select(a => a.ToReadDto(true));
		_logger.LogInformation("Retrieved a query of animal dtos");

		return mappedQuery;
	}

	public IQueryable<AnimalReadDto> GetById(Guid id)
	{
		var mappedQuery = _animalSession.GetById(id).Select(a => a.ToReadDto(true));
		_logger.LogInformation("Retrieved a query of an animal dto.");

		return mappedQuery;
	}

	public async Task DeleteAsync(Guid id)
	{
		var animal = _animalSession.GetByIdOrThrow(id, _logger);

		await _transactionRunner.RunInTransactionAsync(
			() => _animalSession.DeleteAsync(animal),
			_animalSession,
			$"Failed to delete animal with id='{id}'.");

		_logger.LogInformation("Successfully deleted an animal with id={Id}", id);
	}

	public async Task UpdateAsync(Guid id, AnimalUpdateDto dto)
	{
		_ownerSession.GetByIdOrThrow(dto.OwnerId, _logger);

		var animal = _animalSession.GetByIdOrThrow(id, _logger);
		dto.Update(animal);

		await _transactionRunner.RunInTransactionAsync(
			() => _animalSession.UpdateAsync(animal),
			_animalSession,
			$"Failed to update animalQueryable with id='{id}'.");

		_logger.LogInformation("Successfully updated an animal with id={Id}", id);
	}

	public async Task UpdateAsync(Guid id, Delta<AnimalUpdateDto> delta)
	{
		var animal = _animalSession.GetByIdOrThrow(id, _logger);
		var dto = animal.ToUpdateDto();

		_ownerSession.GetByIdOrThrow(dto.OwnerId, _logger);

		delta.Patch(dto);
		var validationResult = await _validator.ValidateAsync(dto);

		if (!validationResult.IsValid)
		{
			throw new ValidationException(validationResult);
		}

		dto.Update(animal);

		await _transactionRunner.RunInTransactionAsync(
			() => _animalSession.UpdateAsync(animal),
			_animalSession,
			$"Failed to update animal with id='{id}'.");

		_logger.LogInformation("Successfully updated an animal with id={Id}", id);
	}
}