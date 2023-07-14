namespace Core.Interfaces;

public interface IMapperSession<T>
{
    void BeginTransaction();
    void CloseTransaction();
    Task CommitAsync();
    Task RollbackAsync();
    IQueryable<T> GetById(Guid id);
    IQueryable<T> GetAll();
    Task SaveAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}
