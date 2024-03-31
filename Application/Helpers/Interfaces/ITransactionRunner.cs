using Domain.Interfaces;

namespace Application.Helpers.Interfaces;

public interface ITransactionRunner
{
	Task RunInTransactionAsync<T>(Func<Task> method, IMapperSession<T> session, string exceptionMessage);
}
