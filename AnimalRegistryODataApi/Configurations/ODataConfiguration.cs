using AnimalRegistryODataApi.Configurations.ConfigurationModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Batch;

namespace AnimalRegistryODataApi.Configurations;

public static class ODataConfiguration
{
    public static void ConfigureOData(this IServiceCollection services)
    {
        services
            .AddControllers(options =>
            {
                options.CacheProfiles.Add("Default", new CacheProfile
                {
                    Duration = 60,
                    VaryByQueryKeys = new[]
                    { 
                        "*"
                    }
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
}
