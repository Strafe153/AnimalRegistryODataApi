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

public class OwnersServiceFixture
{
    public OwnersServiceFixture()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        Context = fixture.Freeze<Mock<AnimalRegistryContext>>();
        Mapper = fixture.Freeze<Mock<IMapper>>();

        OwnersService = new OwnersService(Context.Object, Mapper.Object);

        Id = Guid.NewGuid();
        Owner = GetOwner();
        OwnerDto = GetOwnerDto();
        OwnersDbSet = GetOwnersDbSet();
        OwnerDtosQuery = GetOwnerDtosQuery();
    }

    public OwnersService OwnersService { get; }
    public Mock<AnimalRegistryContext> Context { get; }
    public Mock<IMapper> Mapper { get; }

    public Guid Id { get; }
    public Owner Owner { get; }
    public OwnerDto OwnerDto { get; }
    public DbSet<Owner> OwnersDbSet { get; }
    public IQueryable<OwnerDto> OwnerDtosQuery { get; }

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

    private IList<Owner> GetOwners() =>
        new List<Owner>
        {
            Owner,
            Owner
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

    private IList<OwnerDto> GetOwnerDtos() =>
        new List<OwnerDto>
        {
            OwnerDto,
            OwnerDto
        };

    private DbSet<Owner> GetOwnersDbSet() =>
        GetOwners().AsQueryable().BuildMockDbSet().Object;

    private Animal GetAnimal() =>
        new()
        {
            Id = Guid.NewGuid(),
            PetName = "Morgana",
            Kind = "Not a cat",
            Age = 1,
            Owner = Owner,
            OwnerId = Id
        };

    private IList<Animal> GetAnimals() =>
        new List<Animal>
        {
            GetAnimal(),
            GetAnimal()
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
            GetAnimalDto(),
            GetAnimalDto()
        };

    private IQueryable<OwnerDto> GetOwnerDtosQuery() =>
        GetOwnerDtos().AsQueryable();
}
