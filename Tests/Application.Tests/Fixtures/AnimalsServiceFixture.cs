using Application.AutoMapperProfiles;
using Application.Helpers;
using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using Bogus;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Logging;
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
            .RuleFor(o => o.Age, f => f.Random.Byte())
            .RuleFor(o => o.Email, f => f.Internet.Email())
            .RuleFor(o => o.PhoneNumber, f => f.Phone.PhoneNumber())
            .RuleFor(o => o.Animals, animalFaker.Generate(AnimalsCount));

        var ownerDtoFaker = new Faker<OwnerDto>()
            .RuleFor(o => o.Id, Guid.NewGuid())
            .RuleFor(o => o.FirstName, f => f.Name.FirstName())
            .RuleFor(o => o.LastName, f => f.Name.LastName())
            .RuleFor(o => o.Age, f => f.Random.Byte())
            .RuleFor(o => o.Email, f => f.Internet.Email())
            .RuleFor(o => o.PhoneNumber, f => f.Phone.PhoneNumber())
            .RuleFor(o => o.Animals, animalDtoFaker.Generate(AnimalsCount));

        Id = Guid.NewGuid();

        animalFaker
            .RuleFor(a => a.Id, Id)
            .RuleFor(a => a.PetName, f => f.Name.FirstName())
            .RuleFor(a => a.Kind, f => f.Name.LastName())
            .RuleFor(a => a.Age, f => f.Random.Byte())
            .RuleFor(a => a.Owner, ownerFaker);

        animalDtoFaker
            .RuleFor(a => a.Id, Id)
            .RuleFor(a => a.PetName, f => f.Name.FirstName())
            .RuleFor(a => a.Kind, f => f.Name.LastName())
            .RuleFor(a => a.Age, f => f.Random.Byte())
            .RuleFor(a => a.Owner, ownerDtoFaker)
            .RuleFor(a => a.OwnerId, (f, u) => u.Owner!.Id);

        Session = fixture.Freeze<Mock<IMapperSession<Animal>>>();
        TransactionRunner = fixture.Freeze<Mock<TransactionRunner>>();
        Mapper = new MapperConfiguration(options =>
        {
            options.AddProfiles(new Profile[]
            {
                new AnimalProfile(),
                new OwnerProfile()
            });
        }).CreateMapper();
        Logger = fixture.Freeze<Mock<ILogger<AnimalsService>>>();

        AnimalsService = new AnimalsService(
            Session.Object,
            TransactionRunner.Object,
            Mapper,
            Logger.Object);

        AnimalsCount = Random.Shared.Next(2, 20);
        AnimalDto = animalDtoFaker.Generate();
        GetAllAnimalsQuery = animalFaker.Generate(AnimalsCount).AsQueryable();
        GetByIdAnimalsQuery = animalFaker.Generate(1).AsQueryable();
        GetByIdEmptyAnimalsQuery = animalFaker.Generate(0).AsQueryable();
    }

    public AnimalsService AnimalsService { get; }
    public Mock<IMapperSession<Animal>> Session { get; }
    public Mock<TransactionRunner> TransactionRunner { get; }
    public Mock<ILogger<AnimalsService>> Logger { get; }
    public IMapper Mapper { get; }

    public Guid Id { get; }
    public int AnimalsCount { get; }
    public AnimalDto AnimalDto { get; }
    public IQueryable<Animal> GetAllAnimalsQuery { get; }
    public IQueryable<Animal> GetByIdAnimalsQuery { get; }
    public IQueryable<Animal> GetByIdEmptyAnimalsQuery { get; }
}
