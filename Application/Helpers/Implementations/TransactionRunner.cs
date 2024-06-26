﻿using Application.Helpers.Interfaces;
using Domain.Exceptions;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Helpers.Implementations;

public class TransactionRunner : ITransactionRunner
{
	private readonly ILogger<TransactionRunner> _logger;

	public TransactionRunner(ILogger<TransactionRunner> logger)
	{
		_logger = logger;
	}

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

			_logger.LogWarning(exceptionMessage);
			throw new OperationFailedException(exceptionMessage);
		}
		finally
		{
			session.CloseTransaction();
		}
	}
}
