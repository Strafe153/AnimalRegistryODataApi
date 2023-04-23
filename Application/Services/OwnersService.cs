using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.DTOs;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class OwnersService : IService<OwnerDto>
{
    private readonly AnimalRegistryContext _context;
    private readonly IMapper _mapper;

    public OwnersService(
        AnimalRegistryContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<OwnerDto> CreateAsync(OwnerDto createDto)
    {
        var owner = _mapper.Map<Owner>(createDto);

        try
        {
            _context.Owners.Add(owner);
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw new OperationFailedException($"Failed to create an owner.");
        }

        var readDto = _mapper.Map<OwnerDto>(createDto);
        readDto.Id = owner.Id;

        return readDto;
    }

    public IQueryable<OwnerDto> GetAll() =>
        _mapper.ProjectTo<OwnerDto>(
            _context.Owners.Include(o => o.Animals));

    public IQueryable<OwnerDto> GetById(Guid id) =>
        _mapper.ProjectTo<OwnerDto>(
            _context.Owners
                .Include(o => o.Animals)
                .Where(o => o.Id == id));

    public async Task DeleteAsync(Guid id)
    {
        var owner = await _context.Owners.FindAsync(id);

        if (owner is null)
        {
            throw new NullReferenceException($"Owner with id='{id}' not found.");
        }

        try
        {
            _context.Owners.Remove(owner);
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw new OperationFailedException($"Failed to delete owner with id='{id}'.");
        }
    }

    public async Task UpdateAsync(Guid id, OwnerDto dto)
    {
        var owner = await _context.Owners.FindAsync(id);

        if (owner is null)
        {
            throw new NullReferenceException($"Owner with id='{id}' not found.");
        }

        _mapper.Map(dto, owner);

        try
        {
            _context.Owners.Update(owner);
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw new OperationFailedException($"Failed to update owner with id='{dto.Id}'.");
        }
    }
}
