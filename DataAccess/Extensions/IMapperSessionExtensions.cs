using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace DataAccess.Extensions;

public static class IMapperSessionExtensions
{
	public static T GetByIdOrThrowAsync<T>(this IMapperSession<T> mapperSession, Guid id, ILogger logger)
	{
		var entity = mapperSession.GetById(id).FirstOrDefault();

		if (entity is null)
		{
			var entityName = typeof(T).Name;
			logger.LogWarning("Failed to retrieve an {Name} with id {Id}", entityName, id);

			throw new NullReferenceException($"{entityName} with id='{id}' not found.");
		}

		return entity;
	}
}