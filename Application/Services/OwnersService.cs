using Core.Entities;
using Core.Interfaces;
using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class OwnersService : IService<Owner, Guid>
{
    private readonly AnimalRegistryContext _context;

    public OwnersService(AnimalRegistryContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(Owner entity)
    {
        _context.Owners.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Owner>> GetAllAsync() =>
        await _context.Owners.ToListAsync();

    public async Task<Owner> GetByIdAsync(Guid id)
    {
        var owner = await _context.Owners.FirstOrDefaultAsync(o => o.Id == id);

        if (owner is null)
        {
            throw new NullReferenceException();
        }

        return owner;
    }

    public async Task TaskDeleteAsync(Owner entity)
    {
        _context.Owners.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Owner entity)
    {
        _context.Owners.Update(entity);
        await _context.SaveChangesAsync();
    }
}
