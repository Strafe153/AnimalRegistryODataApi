using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class AnimalsService : IService<AnimalDto>
{
    private readonly AnimalRegistryContext _context;
    private readonly IMapper _mapper;

    public AnimalsService(
        AnimalRegistryContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<AnimalDto> CreateAsync(AnimalDto dto)
    {
        var animal = _mapper.Map<Animal>(dto);

        try
        {
            _context.Animals.Add(animal);
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw new OperationFailedException($"Failed to create an animal.");
        }

        var readDto = _mapper.Map<AnimalDto>(dto);
        readDto.Id = animal.Id;

        return readDto;
    }

    public IQueryable<AnimalDto> GetAll() =>
        _mapper.ProjectTo<AnimalDto>(
            _context.Animals.Include(a => a.Owner));

    public IQueryable<AnimalDto> GetById(Guid id) =>
        _mapper.ProjectTo<AnimalDto>(
            _context.Animals
                .Include(a => a.Owner)
                .Where(a => a.Id == id));

    public async Task DeleteAsync(Guid id)
    {
        var animal = await _context.Animals.FindAsync(id);

        if (animal is null)
        {
            throw new NullReferenceException($"Animal with id='{id}' not found.");
        }

        try
        {
            _context.Animals.Remove(animal);
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw new OperationFailedException($"Failed to delete animal with id='{id}'.");
        }
    }

    public async Task UpdateAsync(Guid id, AnimalDto dto)
    {
        var animal = await _context.Animals.FindAsync(id);

        if (animal is null)
        {
            throw new NullReferenceException($"Animal with id='{id}' not found.");
        }

        _mapper.Map(dto, animal);

        try
        {
            _context.Update(animal);
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw new OperationFailedException($"Failed to update animalQueryable with id='{dto.Id}'.");
        }
    }
}
