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

public class OwnersServiceFixture
{
    public OwnersServiceFixture()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var ownerFaker = new Faker<Owner>();
        var ownerDtoFaker = new Faker<OwnerDto>();

        var animalFaker = new Faker<Animal>()
            .RuleFor(a => a.Id, Guid.NewGuid())
            .RuleFor(a => a.PetName, f => f.Name.FirstName())
            .RuleFor(a => a.Kind, f => f.Name.LastName())
            .RuleFor(a => a.Age, f => f.Random.Int(1, 100))
            .RuleFor(a => a.Owner, ownerFaker)
            .RuleFor(a => a.OwnerId, (f, u) => u.Owner!.Id);

        var animalDtoFaker = new Faker<AnimalDto>()
            .RuleFor(a => a.Id, Guid.NewGuid())
            .RuleFor(a => a.PetName, f => f.Name.FirstName())
            .RuleFor(a => a.Kind, f => f.Name.LastName())
            .RuleFor(a => a.Age, f => f.Random.Int(1, 100))
            .RuleFor(a => a.Owner, ownerDtoFaker)
            .RuleFor(a => a.OwnerId, (f, u) => u.Owner!.Id);

        Id = Guid.NewGuid();

        ownerFaker
            .RuleFor(o => o.Id, Id)
            .RuleFor(o => o.FirstName, f => f.Name.FirstName())
            .RuleFor(o => o.LastName, f => f.Name.LastName())
            .RuleFor(o => o.Age, f => f.Random.Int(1, 100))
            .RuleFor(o => o.Email, f => f.Internet.Email())
            .RuleFor(o => o.PhoneNumber, f => f.Phone.PhoneNumber())
            .RuleFor(o => o.Animals, animalFaker.Generate(OwnersCount));

        ownerDtoFaker
            .RuleFor(o => o.Id, Id)
            .RuleFor(o => o.FirstName, f => f.Name.FirstName())
            .RuleFor(o => o.LastName, f => f.Name.LastName())
            .RuleFor(o => o.Age, f => f.Random.Int(1, 100))
            .RuleFor(o => o.Email, f => f.Internet.Email())
            .RuleFor(o => o.PhoneNumber, f => f.Phone.PhoneNumber())
            .RuleFor(o => o.Animals, animalDtoFaker.Generate(OwnersCount));

        Context = fixture.Freeze<Mock<AnimalRegistryContext>>();
        Mapper = new MapperConfiguration(options =>
        {
            options.AddProfiles(new Profile[]
            {
                new OwnerProfile(),
                new AnimalProfile()
            });
        }).CreateMapper();

        OwnersService = new OwnersService(Context.Object, Mapper);

        OwnersCount = Random.Shared.Next(2, 20);
        Owner = ownerFaker.Generate();
        OwnerDto = ownerDtoFaker.Generate();
        GetAllOwnersDbSet = ownerFaker.Generate(OwnersCount).AsQueryable().BuildMockDbSet().Object;
        GetByIdOwnersDbSet = ownerFaker.Generate(1).AsQueryable().BuildMockDbSet().Object;
    }

    public OwnersService OwnersService { get; }
    public Mock<AnimalRegistryContext> Context { get; }
    public IMapper Mapper { get; }

    public Guid Id { get; }
    public int OwnersCount { get; }
    public Owner Owner { get; }
    public OwnerDto OwnerDto { get; }
    public DbSet<Owner> GetAllOwnersDbSet { get; }
    public DbSet<Owner> GetByIdOwnersDbSet { get; }
}
