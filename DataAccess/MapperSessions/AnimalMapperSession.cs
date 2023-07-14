using Core.Entities;
using Core.Interfaces;
using NHibernate;
using NHibernate.Linq;

namespace DataAccess.MapperSessions;

public class AnimalMapperSession : IMapperSession<Animal>
{
    private readonly ISession _session;
    private ITransaction? _transaction;

    public AnimalMapperSession(ISession session)
    {
        _session = session;
    }

    public void BeginTransaction()
    {
        _transaction = _session.BeginTransaction();
    }

    public void CloseTransaction()
    {
        if (_transaction is not null)
        {
            _transaction.Dispose();
            _transaction = null;
        }
    }

    public async Task CommitAsync()
    {
        if (_transaction is not null)
        {
            await _transaction.CommitAsync();
        }
    }

    public async Task DeleteAsync(Animal entity) =>
        await _session.DeleteAsync(entity);

    public IQueryable<Animal> GetAll() =>
        _session.Query<Animal>().Fetch(a => a.Owner);

    public IQueryable<Animal> GetById(Guid id) =>
        GetAll().Where(a => a.Id == id);

    public async Task RollbackAsync()
    {
        if (_transaction is not null)
        {
            await _transaction.RollbackAsync();
        }
    }

    public async Task SaveAsync(Animal entity) =>
        await _session.SaveAsync(entity);

    public async Task UpdateAsync(Animal entity) =>
        await _session.UpdateAsync(entity);
}
