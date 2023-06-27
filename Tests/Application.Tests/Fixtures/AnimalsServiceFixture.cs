using Application.AutoMapperProfiles;
using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using Bogus;
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

        var animalFaker = new Faker<Animal>();
        var animalDtoFaker = new Faker<AnimalDto>();

        var ownerFaker = new Faker<Owner>()
            .RuleFor(o => o.Id, Guid.NewGuid())
            .RuleFor(o => o.FirstName, f => f.Name.FirstName())
            .RuleFor(o => o.LastName, f => f.Name.LastName())
            .RuleFor(o => o.Age, f => f.Random.Int(1, 100))
            .RuleFor(o => o.Email, f => f.Internet.Email())
            .RuleFor(o => o.PhoneNumber, f => f.Phone.PhoneNumber())
            .RuleFor(o => o.Animals, animalFaker.Generate(AnimalsCount));

        var ownerDtoFaker = new Faker<OwnerDto>()
            .RuleFor(o => o.Id, Guid.NewGuid())
            .RuleFor(o => o.FirstName, f => f.Name.FirstName())
            .RuleFor(o => o.LastName, f => f.Name.LastName())
            .RuleFor(o => o.Age, f => f.Random.Int(1, 100))
            .RuleFor(o => o.Email, f => f.Internet.Email())
            .RuleFor(o => o.PhoneNumber, f => f.Phone.PhoneNumber())
            .RuleFor(o => o.Animals, animalDtoFaker.Generate(AnimalsCount));

        Id = Guid.NewGuid();

        animalFaker
            .RuleFor(a => a.Id, Id)
            .RuleFor(a => a.PetName, f => f.Name.FirstName())
            .RuleFor(a => a.Kind, f => f.Name.LastName())
            .RuleFor(a => a.Age, f => f.Random.Int(1, 100))
            .RuleFor(a => a.Owner, ownerFaker)
            .RuleFor(a => a.OwnerId, (f, u) => u.Owner!.Id);

        animalDtoFaker
            .RuleFor(a => a.Id, Id)
            .RuleFor(a => a.PetName, f => f.Name.FirstName())
            .RuleFor(a => a.Kind, f => f.Name.LastName())
            .RuleFor(a => a.Age, f => f.Random.Int(1, 100))
            .RuleFor(a => a.Owner, ownerDtoFaker)
            .RuleFor(a => a.OwnerId, (f, u) => u.Owner!.Id);

        Context = fixture.Freeze<Mock<AnimalRegistryContext>>();

        Mapper = new MapperConfiguration(options =>
        {
            options.AddProfiles(new Profile[]
            {
                new AnimalProfile(),
                new OwnerProfile()
            });
        }).CreateMapper();

        AnimalsService = new AnimalsService(Context.Object, Mapper);

        AnimalsCount = Random.Shared.Next(2, 20);
        Animal = animalFaker.Generate();
        AnimalDto = animalDtoFaker.Generate();
        GetAllAnimalsDbSet = animalFaker.Generate(AnimalsCount).AsQueryable().BuildMockDbSet().Object;
        GetByIdAnimalsDbSet = animalFaker.Generate(1).AsQueryable().BuildMockDbSet().Object;
    }

    public AnimalsService AnimalsService { get; }
    public Mock<AnimalRegistryContext> Context { get; }
    public IMapper Mapper { get; }

    public Guid Id { get; }
    public int AnimalsCount { get; }
    public Animal Animal { get; }
    public AnimalDto AnimalDto { get; }
    public DbSet<Animal> GetAllAnimalsDbSet { get; }
    public DbSet<Animal> GetByIdAnimalsDbSet { get; }
}
