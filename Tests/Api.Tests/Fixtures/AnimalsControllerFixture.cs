using AnimalRegistryODataApi.Controllers;
using AutoFixture;
using AutoFixture.AutoMoq;
using Bogus;
using Domain.DTOs;
using Domain.Interfaces;
using Microsoft.AspNetCore.OData.Deltas;
using Moq;

namespace Api.Tests.Fixtures;

public class AnimalsControllerFixture
{
    public AnimalsControllerFixture()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var animalDtoFaker = new Faker<AnimalDto>();

        var ownerDtoFaker = new Faker<OwnerDto>()
            .RuleFor(o => o.Id, Guid.NewGuid())
            .RuleFor(o => o.FirstName, f => f.Name.FirstName())
            .RuleFor(o => o.LastName, f => f.Name.LastName())
            .RuleFor(o => o.Age, f => f.Random.Byte())
            .RuleFor(o => o.Email, f => f.Internet.Email())
            .RuleFor(o => o.PhoneNumber, f => f.Phone.PhoneNumber())
            .RuleFor(o => o.Animals, animalDtoFaker.Generate(AnimalDtosCount));

        animalDtoFaker
            .RuleFor(a => a.Id, Id)
            .RuleFor(a => a.PetName, f => f.Name.FirstName())
            .RuleFor(a => a.Kind, f => f.Name.LastName())
            .RuleFor(a => a.Age, f => f.Random.Byte())
            .RuleFor(a => a.Owner, ownerDtoFaker)
            .RuleFor(a => a.OwnerId, (f, u) => u.Owner!.Id);

        AnimalsService = fixture.Freeze<Mock<IService<AnimalDto>>>();

        AnimalsController = new AnimalsController(AnimalsService.Object);

        Id = Guid.NewGuid();
        AnimalDtosCount = Random.Shared.Next(2, 20);
        AnimalDto = animalDtoFaker.Generate();
        AnimalDtoDelta = new Delta<AnimalDto>();
        AnimalDtos = animalDtoFaker.Generate(AnimalDtosCount);
        AnimalDtoQuery = AnimalDtos.AsQueryable();
    }

    public AnimalsController AnimalsController { get; }
    public Mock<IService<AnimalDto>> AnimalsService { get; }

    public Guid Id { get; }
    public int AnimalDtosCount { get; }
    public AnimalDto AnimalDto { get; }
    public Delta<AnimalDto> AnimalDtoDelta { get; }
    public IList<AnimalDto> AnimalDtos { get; }
    public IQueryable<AnimalDto> AnimalDtoQuery { get; }
}
