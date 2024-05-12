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

public class OwnersService : IOwnersService
{
	private readonly IMapperSession<Owner> _session;
	private readonly ITransactionRunner _transactionRunner;
	private readonly IMapper _mapper;
	private readonly ILogger<OwnersService> _logger;

	public OwnersService(
		IMapperSession<Owner> session,
		ITransactionRunner transactionRunner,
		IMapper mapper,
		ILogger<OwnersService> logger)
	{
		_session = session;
		_transactionRunner = transactionRunner;
		_mapper = mapper;
		_logger = logger;
	}

	public async Task<OwnerDto> CreateAsync(OwnerDto createDto)
	{
		var owner = _mapper.Map<Owner>(createDto);

		await _transactionRunner.RunInTransactionAsync(
			() => _session.SaveAsync(owner),
			_session,
			$"Failed to create an owner.");

		_logger.LogInformation("Successfully created an owner");

		var readDto = _mapper.Map<OwnerDto>(createDto);
		readDto.Id = owner.Id;

		return readDto;
	}

	public IQueryable<OwnerDto> GetAll()
	{
		var mappedQuery = _mapper.ProjectTo<OwnerDto>(_session.GetAll());
		_logger.LogInformation("Retrieved a query of owner dtos");

		return mappedQuery;
	}

	public IQueryable<OwnerDto> GetById(Guid id)
	{
		var mappedQuery = _mapper.ProjectTo<OwnerDto>(_session.GetById(id));
		_logger.LogInformation("Retrieved a query of an owner dto.");

		return mappedQuery;
	}

	public async Task DeleteAsync(Guid id)
	{
		var owner = _session.GetByIdOrThrowAsync(id, _logger);

		await _transactionRunner.RunInTransactionAsync(
			() => _session.DeleteAsync(owner),
			_session,
			$"Failed to delete owner with id='{id}'.");

		_logger.LogInformation("Successfully deleted an owner with id={Id}", id);
	}

	public async Task UpdateAsync(Guid id, OwnerDto dto)
	{
		var owner = _session.GetByIdOrThrowAsync(id, _logger);
		_mapper.Map(dto, owner);

		await _transactionRunner.RunInTransactionAsync(
			() => _session.UpdateAsync(owner),
			_session,
			$"Failed to update owner with id='{id}'.");

		_logger.LogInformation("Successfully updated an owner with id={Id}", id);
	}

	public async Task UpdateAsync(Guid id, Delta<OwnerDto> delta)
	{
		var owner = _session.GetByIdOrThrowAsync(id, _logger);
		var dto = _mapper.Map<OwnerDto>(owner);

		delta.Patch(dto);
		_mapper.Map(dto, owner);

		await _transactionRunner.RunInTransactionAsync(
			() => _session.UpdateAsync(owner),
			_session,
			$"Failed to update owner with id='{id}'.");

		_logger.LogInformation("Successfully updated an owner with id={Id}", id);
	}
}