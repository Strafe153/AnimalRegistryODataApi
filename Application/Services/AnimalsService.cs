using Application.Helpers;
using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces;

namespace Application.Services;

public class AnimalsService : IService<AnimalDto>
{
    private readonly IMapperSession<Animal> _session;
    private readonly TransactionRunner _transactionRunner;
    private readonly IMapper _mapper;

    public AnimalsService(
        IMapperSession<Animal> session,
        TransactionRunner transactionRunner,
        IMapper mapper)
    {
        _session = session;
        _transactionRunner = transactionRunner;
        _mapper = mapper;
    }

    public async Task<AnimalDto> CreateAsync(AnimalDto dto)
    {
        var animal = _mapper.Map<Animal>(dto);

        await _transactionRunner.RunInTransactionAsync(
            async () => await _session.SaveAsync(animal),
            _session,
            $"Failed to create an animal.");

        var readDto = _mapper.Map<AnimalDto>(dto);
        readDto.Id = animal.Id;

        return readDto;
    }

    public IQueryable<AnimalDto> GetAll() =>
        _mapper.ProjectTo<AnimalDto>(_session.GetAll());

    public IQueryable<AnimalDto> GetById(Guid id) =>
        _mapper.ProjectTo<AnimalDto>(_session.GetById(id));

    public async Task DeleteAsync(Guid id)
    {
        var animal = _session.GetById(id).FirstOrDefault();

        if (animal is null)
        {
            throw new NullReferenceException($"Animal with id='{id}' not found.");
        }

        await _transactionRunner.RunInTransactionAsync(
            async () => await _session.DeleteAsync(animal),
            _session,
            $"Failed to delete animal with id='{id}'.");
    }

    public async Task UpdateAsync(Guid id, AnimalDto dto)
    {
        var animal = _session.GetById(id).FirstOrDefault();

        if (animal is null)
        {
            throw new NullReferenceException($"Animal with id='{id}' not found.");
        }

        _mapper.Map(dto, animal);

        await _transactionRunner.RunInTransactionAsync(
            async () => await _session.UpdateAsync(animal),
            _session,
            $"Failed to update animalQueryable with id='{dto.Id}'.");
    }
}