using Microsoft.AspNetCore.OData.Deltas;

namespace Core.Interfaces;

public interface IService<T> where T : class
{
    IQueryable<T> GetAll();
    IQueryable<T> GetById(Guid id);
    Task<T> CreateAsync(T dto);
    Task UpdateAsync(Guid id, T dto);
    Task UpdateAsync(Guid id, Delta<T> delta);
    Task DeleteAsync(Guid id);
}
