using Application.DTOs;
using Domain.Entities;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace AnimalRegistryODataApi.Configurations.Models;

public static class ODataEdmModelBuilder
{
	public static IEdmModel BuildV1EdmModel()
	{
		var v1ModelBuilder = new ODataConventionModelBuilder();
		v1ModelBuilder.EntitySet<OwnerDto>($"{nameof(Owner)}s");
		v1ModelBuilder.EntitySet<AnimalDto>($"{nameof(Animal)}s");

		return v1ModelBuilder.GetEdmModel();
	}
}
