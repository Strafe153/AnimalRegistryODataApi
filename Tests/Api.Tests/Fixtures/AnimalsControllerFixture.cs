using AnimalRegistryODataApi.Controllers;
using AutoFixture.AutoMoq;
using AutoFixture;
using Core.DTOs;
using Core.Interfaces;
using Moq;

namespace Api.Tests.Fixtures;

public class AnimalsControllerFixture
{
    public AnimalsControllerFixture()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        AnimalsService = fixture.Freeze<Mock<IService<AnimalDto>>>();

        AnimalsController = new AnimalsController(AnimalsService.Object);

        Id = Guid.NewGuid();
        AnimalDto = GetAnimalDto();
        AnimalDtos = GetAnimalDtos();
        AnimalDtoQuery = GetAnimalDtoQuery();
    }

    public AnimalsController AnimalsController { get; }
    public Mock<IService<AnimalDto>> AnimalsService { get; }

    public Guid Id { get; }
    public AnimalDto AnimalDto { get; }
    public IList<AnimalDto> AnimalDtos { get; }
    public IQueryable<AnimalDto> AnimalDtoQuery { get; }

    private OwnerDto GetOwnerDto() =>
        new()
        {
            Id = Id,
            FirstName = "Ren",
            LastName = "Amamiya",
            Age = 16,
            Email = "joker@gmail.com",
            PhoneNumber = "0662931093",
            Animals = GetAnimalDtos()
        };

    private AnimalDto GetAnimalDto() =>
        new()
        {
            Id = Guid.NewGuid(),
            PetName = "Morgana",
            Kind = "Not a cat",
            Age = 1,
            Owner = GetOwnerDto(),
            OwnerId = Id
        };

    private IList<AnimalDto> GetAnimalDtos() =>
        new List<AnimalDto>
        {
            AnimalDto,
            AnimalDto
        };

    private IQueryable<AnimalDto> GetAnimalDtoQuery() =>
        AnimalDtos.AsQueryable();
}
