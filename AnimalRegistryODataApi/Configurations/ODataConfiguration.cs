using AnimalRegistryODataApi.Configurations.Models;
using Domain.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Batch;

namespace AnimalRegistryODataApi.Configurations;

public static class ODataConfiguration
{
	public static void ConfigureOData(this IServiceCollection services, IConfiguration configuration) =>
		services
			.AddControllers(options =>
			{
				var cacheOptions = configuration.GetSection(CacheOptions.SectionName).Get<CacheOptions>()!;

				options.CacheProfiles.Add(CacheConstants.Default, new CacheProfile
				{
					Location = ResponseCacheLocation.Any,
					Duration = cacheOptions.Duration,
					VaryByQueryKeys = ["*"]
				});
			})
			.AddOData(options =>
			{
				options.EnableQueryFeatures(null);
				options.AddRouteComponents(
					"odata/v1",
					ODataEdmModelBuilder.BuildV1EdmModel(),
					new DefaultODataBatchHandler());
			});
}
