using Core.Entities;
using Core.Interfaces;
using NHibernate;
using NHibernate.Linq;

namespace DataAccess.MapperSessions;

public class OwnerMapperSession : IMapperSession<Owner>
{
    private readonly ISession _session;
    private ITransaction? _transaction;

    public OwnerMapperSession(ISession session)
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

    public async Task DeleteAsync(Owner entity) =>
        await _session.DeleteAsync(entity);

    public IQueryable<Owner> GetAll() =>
        _session.Query<Owner>()
            .Fetch(o => o.Animals)
            .ToFuture()
            .AsQueryable();

    public IQueryable<Owner> GetById(Guid id) =>
        GetAll().Where(o => o.Id == id);

    public async Task RollbackAsync()
    {
        if (_transaction is not null)
        {
            await _transaction.RollbackAsync();
        }
    }

    public async Task SaveAsync(Owner entity) =>
        await _session.SaveAsync(entity);

    public async Task UpdateAsync(Owner entity) =>
        await _session.UpdateAsync(entity);
}
