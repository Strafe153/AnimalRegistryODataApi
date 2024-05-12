using Application.DTOs;
using Application.Helpers.Interfaces;
using Application.Services.Interfaces;
using AutoMapper;
using DataAccess.Extensions;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.Extensions.Logging;

namespace Application.Services.Implementations;

public class AnimalsService : IAnimalsService
{
	private readonly IMapperSession<Animal> _animalSession;
	private readonly IMapperSession<Owner> _ownerSession;
	private readonly ITransactionRunner _transactionRunner;
	private readonly IMapper _mapper;
	private readonly ILogger<AnimalsService> _logger;

	public AnimalsService(
		IMapperSession<Animal> animalSession,
		IMapperSession<Owner> ownerSession,
		ITransactionRunner transactionRunner,
		IMapper mapper,
		ILogger<AnimalsService> logger)
	{
		_animalSession = animalSession;
		_ownerSession = ownerSession;
		_transactionRunner = transactionRunner;
		_mapper = mapper;
		_logger = logger;
	}

	public async Task<AnimalDto> CreateAsync(AnimalDto dto)
	{
		_ownerSession.GetByIdOrThrowAsync(dto.OwnerId, _logger);

		var animal = _mapper.Map<Animal>(dto);

		await _transactionRunner.RunInTransactionAsync(
			() => _animalSession.SaveAsync(animal),
			_animalSession,
			$"Failed to create an animal.");

		_logger.LogInformation("Successfully created an animal");

		var readDto = _mapper.Map<AnimalDto>(dto);
		readDto.Id = animal.Id;

		return readDto;
	}

	public IQueryable<AnimalDto> GetAll()
	{
		var mappedQuery = _mapper.ProjectTo<AnimalDto>(_animalSession.GetAll());
		_logger.LogInformation("Retrieved a query of animal dtos");

		return mappedQuery;
	}

	public IQueryable<AnimalDto> GetById(Guid id)
	{
		var mappedQuery = _mapper.ProjectTo<AnimalDto>(_animalSession.GetById(id));
		_logger.LogInformation("Retrieved a query of an animal dto.");

		return mappedQuery;
	}

	public async Task DeleteAsync(Guid id)
	{
		var animal = _animalSession.GetByIdOrThrowAsync(id, _logger);

		await _transactionRunner.RunInTransactionAsync(
			() => _animalSession.DeleteAsync(animal),
			_animalSession,
			$"Failed to delete animal with id='{id}'.");

		_logger.LogInformation("Successfully deleted an animal with id={Id}", id);
	}

	public async Task UpdateAsync(Guid id, AnimalDto dto)
	{
		_ownerSession.GetByIdOrThrowAsync(dto.OwnerId, _logger);

		var animal = _animalSession.GetByIdOrThrowAsync(id, _logger);
		_mapper.Map(dto, animal);

		await _transactionRunner.RunInTransactionAsync(
			() => _animalSession.UpdateAsync(animal),
			_animalSession,
			$"Failed to update animalQueryable with id='{id}'.");

		_logger.LogInformation("Successfully updated an animal with id={Id}", id);
	}

	public async Task UpdateAsync(Guid id, Delta<AnimalDto> delta)
	{
		var animal = _animalSession.GetByIdOrThrowAsync(id, _logger);
		var dto = _mapper.Map<AnimalDto>(animal);

		delta.Patch(dto);

		_ownerSession.GetByIdOrThrowAsync(dto.OwnerId, _logger);
		_mapper.Map(dto, animal);

		await _transactionRunner.RunInTransactionAsync(
			() => _animalSession.UpdateAsync(animal),
			_animalSession,
			$"Failed to update animal with id='{id}'.");

		_logger.LogInformation("Successfully updated an animal with id={Id}", id);
	}
}