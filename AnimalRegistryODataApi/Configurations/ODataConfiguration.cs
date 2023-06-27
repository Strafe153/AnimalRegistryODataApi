using Core.DTOs;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;

namespace AnimalRegistryODataApi.Configurations;

public static class ODataConfiguration
{
    public static void ConfigureOData(this IServiceCollection services)
    {
        var modelBuilder = new ODataConventionModelBuilder();
        modelBuilder.EntitySet<OwnerDto>("Owners");
        modelBuilder.EntitySet<AnimalDto>("Animals");

        var edmModel = modelBuilder.GetEdmModel();

        services
            .AddControllers()
            .AddOData(options =>
                options
                    .EnableQueryFeatures(null)
                    .AddRouteComponents("odata", edmModel));
    }
}
