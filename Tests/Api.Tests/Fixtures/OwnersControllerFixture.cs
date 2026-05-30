using AnimalRegistryODataApi.Controllers;
using Application.Services.Interfaces;
using Moq;

namespace Api.Tests.Fixtures;

public class OwnersControllerFixture
{
	public Mock<IOwnersService> OwnersService { get; } = new();

	public OwnersController CreateSut() => new(OwnersService.Object);
}
