using AnimalRegistryODataApi.Configurations.ConfigurationModels;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Batch;

namespace AnimalRegistryODataApi.Configurations;

public static class ODataConfiguration
{
    public static void ConfigureOData(this IServiceCollection services)
    {
        services
            .AddControllers()
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
