namespace Core.Interfaces;

public interface IService<T, TId>
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(TId id);
    Task CreateAsync(T entity);
    Task UpdateAsync(T entity);
    Task TaskDeleteAsync(T entity);
}
