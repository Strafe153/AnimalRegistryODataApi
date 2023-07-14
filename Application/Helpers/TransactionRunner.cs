using Core.Exceptions;
using Core.Interfaces;

namespace Application.Helpers;

public class TransactionRunner
{
    public virtual async Task RunInTransactionAsync<T>(
        Func<Task> method,
        IMapperSession<T> session,
        string exceptionMessage)
    {
        try
        {
            session.BeginTransaction();
            await method();
            await session.CommitAsync();
        }
        catch
        {
            await session.RollbackAsync();
            throw new OperationFailedException(exceptionMessage);
        }
        finally
        {
            session.CloseTransaction();
        }
    }
}
