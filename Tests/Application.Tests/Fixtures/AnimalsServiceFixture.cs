using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using Core.DTOs;
using Core.Entities;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;

namespace Application.Tests.Fixtures;

public class AnimalsServiceFixture
{
    public AnimalsServiceFixture()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        Context = fixture.Freeze<Mock<AnimalRegistryContext>>();
        Mapper = fixture.Freeze<Mock<IMapper>>();

        AnimalsService = new AnimalsService(Context.Object, Mapper.Object);

        Id = Guid.NewGuid();
        Animal = GetAnimal();
        AnimalDto = GetAnimalDto();
        AnimalsDbSet = GetAnimalsDbSet();
        AnimalDtosQuery = GetAnimalDtosQuery();
    }

    public AnimalsService AnimalsService { get; }
    public Mock<AnimalRegistryContext> Context { get; }
    public Mock<IMapper> Mapper { get; }

    public Guid Id { get; }
    public Animal Animal { get; }
    public AnimalDto AnimalDto { get; }
    public DbSet<Animal> AnimalsDbSet { get; }
    public IQueryable<AnimalDto> AnimalDtosQuery { get; }

    private Owner GetOwner() =>
        new()
        {
            Id = Id,
            FirstName = "Ren",
            LastName = "Amamiya",
            Age = 16,
            Email = "joker@gmail.com",
            PhoneNumber = "0662931093",
            Animals = GetAnimals()
        };

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

    private IList<Animal> GetAnimals() =>
        new List<Animal>
        {
            Animal,
            Animal
        };

    private Animal GetAnimal() =>
        new()
        {
            Id = Guid.NewGuid(),
            PetName = "Morgana",
            Kind = "Not a cat",
            Age = 1,
            Owner = GetOwner(),
            OwnerId = Id
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

    private DbSet<Animal> GetAnimalsDbSet() =>
        GetAnimals().AsQueryable().BuildMockDbSet().Object;

    private IQueryable<AnimalDto> GetAnimalDtosQuery() =>
        GetAnimalDtos().AsQueryable();
}
