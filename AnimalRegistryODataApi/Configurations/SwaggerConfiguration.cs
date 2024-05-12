using System.Reflection;

namespace AnimalRegistryODataApi.Configurations;

public static class SwaggerConfiguration
{
	public static void ConfigureSwagger(this IServiceCollection services) =>
		services.AddSwaggerGen(options =>
		{
			var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
			options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
		});
}
