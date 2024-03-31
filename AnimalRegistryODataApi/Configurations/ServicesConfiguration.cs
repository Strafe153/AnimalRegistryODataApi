using Application.Helpers.Implementations;
using Application.Helpers.Interfaces;
using Application.Services.Implementations;
using Application.Services.Interfaces;

namespace AnimalRegistryODataApi.Configurations;

public static class ServicesConfiguration
{
	public static void ConfigureServices(this IServiceCollection services) =>
		services
			.AddScoped<IOwnersService, OwnersService>()
			.AddScoped<IAnimalsService, AnimalsService>()
			.AddScoped<ITransactionRunner, TransactionRunner>();
}
