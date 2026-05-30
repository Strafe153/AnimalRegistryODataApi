using Application.DTOs.Animal;
using Application.DTOs.Owner;
using Domain.Entities;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace AnimalRegistryODataApi.Configurations.Models;

public static class ODataEdmModelBuilder
{
	public static IEdmModel BuildV1EdmModel()
	{
		ODataConventionModelBuilder v1ModelBuilder = new();

		v1ModelBuilder.EntitySet<OwnerReadDto>($"{nameof(Owner)}s");
		v1ModelBuilder.EntitySet<AnimalReadDto>($"{nameof(Animal)}s");

		v1ModelBuilder.ComplexType<OwnerUpdateDto>();
		v1ModelBuilder.ComplexType<AnimalUpdateDto>();

		return v1ModelBuilder.GetEdmModel();
	}
}
