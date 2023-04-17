namespace Core.Interfaces;

public interface IService<T>
{
    IQueryable<T> GetAllAsync();
    IQueryable<T> GetByIdAsync(Guid id);
    Task<T> CreateAsync(T dto);
    Task UpdateAsync(Guid id, T dto);
    Task DeleteAsync(Guid id);
}
