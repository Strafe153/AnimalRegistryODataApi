using AnimalRegistryODataApi.Middleware;

namespace AnimalRegistryODataApi.Configurations;

public static class MiddlewareConfiguration
{
	public static void ConfigureMiddleware(this IServiceCollection services) =>
		services.AddSingleton<ExceptionHandlingMiddleware>();
}
