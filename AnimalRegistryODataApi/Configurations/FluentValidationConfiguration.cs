using AnimalRegistryODataApi.Validators;
using Application.DTOs;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace AnimalRegistryODataApi.Configurations;

public static class FluentValidationConfiguration
{
	public static void ConfigureFluentValidation(this IServiceCollection services)
	{
		services.AddCustomValidators();

		services
			.AddFluentValidationAutoValidation()
			.AddFluentValidationClientsideAdapters();
	}

	private static void AddCustomValidators(this IServiceCollection services) =>
		services
			.AddScoped<IValidator<OwnerDto>, OwnerValidator>()
			.AddScoped<IValidator<AnimalDto>, AnimalValidator>();
}
