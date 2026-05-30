using Application.DTOs.Owner;
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

public class OwnersService : IOwnersService
{
	private readonly IMapperSession<Owner> _session;
	private readonly ITransactionRunner _transactionRunner;
	private readonly IValidator<OwnerUpdateDto> _validator;
	private readonly ILogger<OwnersService> _logger;

	public OwnersService(
		IMapperSession<Owner> session,
		ITransactionRunner transactionRunner,
		IValidator<OwnerUpdateDto> validator,
		ILogger<OwnersService> logger)
	{
		_session = session;
		_transactionRunner = transactionRunner;
		_validator = validator;
		_logger = logger;
	}

	public async Task<OwnerReadDto> CreateAsync(OwnerCreateDto createDto)
	{
		var owner = createDto.ToOwner();

		await _transactionRunner.RunInTransactionAsync(
			() => _session.SaveAsync(owner),
			_session,
			"Failed to create an owner.");

		_logger.LogInformation("Successfully created an owner");

		return owner.ToReadDto();
	}

	public IQueryable<OwnerReadDto> GetAll()
	{
		var mappedQuery = _session.GetAll().Select(o => o.ToReadDto(true));
		_logger.LogInformation("Retrieved a query of owner dtos");

		return mappedQuery;
	}

	public IQueryable<OwnerReadDto> GetById(Guid id)
	{
		var mappedQuery = _session.GetById(id).Select(o => o.ToReadDto(true));
		_logger.LogInformation("Retrieved a query of an owner dto.");

		return mappedQuery;
	}

	public async Task DeleteAsync(Guid id)
	{
		var owner = _session.GetByIdOrThrow(id, _logger);

		await _transactionRunner.RunInTransactionAsync(
			() => _session.DeleteAsync(owner),
			_session,
			$"Failed to delete owner with id='{id}'.");

		_logger.LogInformation("Successfully deleted an owner with id={Id}", id);
	}

	public async Task UpdateAsync(Guid id, OwnerUpdateDto dto)
	{
		var owner = _session.GetByIdOrThrow(id, _logger);
		dto.Update(owner);

		await _transactionRunner.RunInTransactionAsync(
			() => _session.UpdateAsync(owner),
			_session,
			$"Failed to update owner with id='{id}'.");

		_logger.LogInformation("Successfully updated an owner with id={Id}", id);
	}

	public async Task UpdateAsync(Guid id, Delta<OwnerUpdateDto> delta)
	{
		var owner = _session.GetByIdOrThrow(id, _logger);
		var dto = owner.ToUpdateDto();

		delta.Patch(dto);
		var validationResult = await _validator.ValidateAsync(dto);

		if (!validationResult.IsValid)
		{
			throw new ValidationException(validationResult);
		}

		dto.Update(owner);

		await _transactionRunner.RunInTransactionAsync(
			() => _session.UpdateAsync(owner),
			_session,
			$"Failed to update owner with id='{id}'.");

		_logger.LogInformation("Successfully updated an owner with id={Id}", id);
	}
}