using Application.Helpers;
using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces;

namespace Application.Services;

public class OwnersService : IService<OwnerDto>
{
    private readonly IMapperSession<Owner> _session;
    private readonly TransactionRunner _transactionRunner;
    private readonly IMapper _mapper;

    public OwnersService(
        IMapperSession<Owner> session,
        TransactionRunner transactionRunner,
        IMapper mapper)
    {
        _session = session;
        _transactionRunner = transactionRunner;
        _mapper = mapper;
    }

    public async Task<OwnerDto> CreateAsync(OwnerDto createDto)
    {
        var owner = _mapper.Map<Owner>(createDto);

        await _transactionRunner.RunInTransactionAsync(
            async () => await _session.SaveAsync(owner),
            _session,
            $"Failed to create an owner.");

        var readDto = _mapper.Map<OwnerDto>(createDto);
        readDto.Id = owner.Id;

        return readDto;
    }

    public IQueryable<OwnerDto> GetAll() =>
        _mapper.ProjectTo<OwnerDto>(_session.GetAll());

    public IQueryable<OwnerDto> GetById(Guid id) =>
        _mapper.ProjectTo<OwnerDto>(_session.GetById(id));

    public async Task DeleteAsync(Guid id)
    {
        var owner = _session.GetById(id).FirstOrDefault();

        if (owner is null)
        {
            throw new NullReferenceException($"Owner with id='{id}' not found.");
        }

        await _transactionRunner.RunInTransactionAsync(
            async () => await _session.DeleteAsync(owner),
            _session,
            $"Failed to delete owner with id='{id}'.");
    }

    public async Task UpdateAsync(Guid id, OwnerDto dto)
    {
        var owner = _session.GetById(id).FirstOrDefault();

        if (owner is null)
        {
            throw new NullReferenceException($"Owner with id='{id}' not found.");
        }

        _mapper.Map(dto, owner);

        await _transactionRunner.RunInTransactionAsync(
            async () => await _session.UpdateAsync(owner),
            _session,
            $"Failed to update owner with id='{dto.Id}'.");
    }
}
