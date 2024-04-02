﻿using Application.DTOs;
using Application.Helpers.Interfaces;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.Extensions.Logging;

namespace Application.Services.Implementations;

public class AnimalsService : IAnimalsService
{
	private readonly IMapperSession<Animal> _session;
	private readonly ITransactionRunner _transactionRunner;
	private readonly IMapper _mapper;
	private readonly ILogger<AnimalsService> _logger;

	public AnimalsService(
		IMapperSession<Animal> session,
		ITransactionRunner transactionRunner,
		IMapper mapper,
		ILogger<AnimalsService> logger)
	{
		_session = session;
		_transactionRunner = transactionRunner;
		_mapper = mapper;
		_logger = logger;
	}

	public async Task<AnimalDto> CreateAsync(AnimalDto dto)
	{
		var animal = _mapper.Map<Animal>(dto);

		await _transactionRunner.RunInTransactionAsync(
			async () => await _session.SaveAsync(animal),
			_session,
			$"Failed to create an animal.");

		_logger.LogInformation("Successfully created an animal");

		var readDto = _mapper.Map<AnimalDto>(dto);
		readDto.Id = animal.Id;

		return readDto;
	}

	public IQueryable<AnimalDto> GetAll()
	{
		var mappedQuery = _mapper.ProjectTo<AnimalDto>(_session.GetAll());
		_logger.LogInformation("Retrieved a query of animal dtos");

		return mappedQuery;
	}

	public IQueryable<AnimalDto> GetById(Guid id)
	{
		var mappedQuery = _mapper.ProjectTo<AnimalDto>(_session.GetById(id));
		_logger.LogInformation("Retrieved a query of an animal dto.");

		return mappedQuery;
	}

	public async Task DeleteAsync(Guid id)
	{
		var animal = _session.GetById(id).FirstOrDefault();

		if (animal is null)
		{
			_logger.LogWarning("Failed to retrieve a spell with id {Id}", id);
			throw new NullReferenceException($"Animal with id='{id}' not found.");
		}

		await _transactionRunner.RunInTransactionAsync(
			async () => await _session.DeleteAsync(animal),
			_session,
			$"Failed to delete animal with id='{id}'.");

		_logger.LogInformation("Successfully deleted an animal with id={Id}", id);
	}

	public async Task UpdateAsync(Guid id, AnimalDto dto)
	{
		var animal = _session.GetById(id).FirstOrDefault();

		if (animal is null)
		{
			_logger.LogWarning("Failed to retrieve an animal with id {Id}", id);
			throw new NullReferenceException($"Animal with id='{id}' not found.");
		}

		_mapper.Map(dto, animal);

		await _transactionRunner.RunInTransactionAsync(
			async () => await _session.UpdateAsync(animal),
			_session,
			$"Failed to update animalQueryable with id='{id}'.");

		_logger.LogInformation("Successfully updated an animal with id={Id}", id);
	}

	public async Task UpdateAsync(Guid id, Delta<AnimalDto> delta)
	{
		var animal = _session.GetById(id).FirstOrDefault();

		if (animal is null)
		{
			_logger.LogWarning("Failed to retrieve an animal with id {Id}", id);
			throw new NullReferenceException($"Animal with id='{id}' not found.");
		}

		var dto = _mapper.Map<AnimalDto>(animal);

		delta.Patch(dto);
		_mapper.Map(dto, animal);

		await _transactionRunner.RunInTransactionAsync(
			async () => await _session.UpdateAsync(animal),
			_session,
			$"Failed to update animal with id='{id}'.");

		_logger.LogInformation("Successfully updated an animal with id={Id}", id);
	}
}