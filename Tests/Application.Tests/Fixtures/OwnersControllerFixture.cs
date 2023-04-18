using AnimalRegistryODataApi.Controllers;
using AutoFixture;
using AutoFixture.AutoMoq;
using Core.DTOs;
using Core.Interfaces;
using Moq;

namespace Application.Tests.Fixtures;

public class OwnersControllerFixture
{
    public OwnersControllerFixture()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        OwnersService = fixture.Freeze<Mock<IService<OwnerDto>>>();

        OwnersController = new OwnersController(OwnersService.Object);

        Id = Guid.NewGuid();
        OwnerDto = GetOwnerDto();
        AnimalDto = GetAnimalDto();
        OwnerDtos = GetOwnerDtos();
        OwnerDtoQuery = GetOwnerDtoQuery();
    }

    public OwnersController OwnersController { get; }
    public Mock<IService<OwnerDto>> OwnersService { get; }

    public Guid Id { get; }
    public OwnerDto OwnerDto { get; }
    public AnimalDto AnimalDto { get; }
    public IList<OwnerDto> OwnerDtos { get; }
    public IQueryable<OwnerDto> OwnerDtoQuery { get; }

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

    private IList<OwnerDto> GetOwnerDtos() =>
        new List<OwnerDto>
        {
            OwnerDto,
            OwnerDto
        };

    private AnimalDto GetAnimalDto() =>
        new()
        {
            Id = Guid.NewGuid(),
            PetName = "Morgana",
            Kind = "Not a cat",
            Age = 1,
            Owner = OwnerDto,
            OwnerId = Id
        };

    private IList<AnimalDto> GetAnimalDtos() =>
        new List<AnimalDto>
        {
            AnimalDto,
            AnimalDto
        };

    private IQueryable<OwnerDto> GetOwnerDtoQuery() =>
        OwnerDtos.AsQueryable();
}
