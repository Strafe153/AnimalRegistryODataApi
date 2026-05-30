using AnimalRegistryODataApi.Controllers;
using Application.Services.Interfaces;
using Moq;

namespace Api.Tests.Fixtures;

public class AnimalsControllerFixture
{
	public Mock<IAnimalsService> AnimalsService { get; } = new();

	public AnimalsController CreateSut() => new(AnimalsService.Object);
}
