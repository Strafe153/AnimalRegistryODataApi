using AnimalRegistryODataApi.Middleware;

namespace AnimalRegistryODataApi.Configurations;

public static class MiddlewareConfiguration
{
    public static void UseCustomMiddleware(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<ExceptionsMiddleware>();
    }
}
