using Core.DTOs;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace AnimalRegistryODataApi.Configurations.ConfigurationModels;

public static class ODataEdmModelBuilder
{
    public static IEdmModel BuildV1EdmModel()
    {
        var v1ModelBuilder = new ODataConventionModelBuilder();
        v1ModelBuilder.EntitySet<OwnerDto>("Owners");
        v1ModelBuilder.EntitySet<AnimalDto>("Animals");

        return v1ModelBuilder.GetEdmModel();
    }
}
