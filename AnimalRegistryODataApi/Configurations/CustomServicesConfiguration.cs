using Application.Services;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces;

namespace AnimalRegistryODataApi.Configurations;

public static class CustomServicesConfiguration
{
    public static void ConfigureCustomServices(this IServiceCollection services)
    {
        services.AddScoped<IService<OwnerDto>, OwnersService>();
        services.AddScoped<IService<AnimalDto>, AnimalsService>();
    }
}
