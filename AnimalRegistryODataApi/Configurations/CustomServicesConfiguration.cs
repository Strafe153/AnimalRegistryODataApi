using Application.Helpers;
using Application.Services;
using Domain.DTOs;
using Domain.Interfaces;

namespace AnimalRegistryODataApi.Configurations;

public static class CustomServicesConfiguration
{
	public static void ConfigureCustomServices(this IServiceCollection services) =>
		services
			.AddScoped<IService<OwnerDto>, OwnersService>()
			.AddScoped<IService<AnimalDto>, AnimalsService>()
			.AddScoped<TransactionRunner>();
}
