﻿using Application.AutoMapperProfiles;
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
            .RuleFor(a => a.Age, f => f.Random.Byte())
            .RuleFor(a => a.Owner, ownerFaker);

        var animalDtoFaker = new Faker<AnimalDto>()
            .RuleFor(a => a.Id, Guid.NewGuid())
            .RuleFor(a => a.PetName, f => f.Name.FirstName())
            .RuleFor(a => a.Kind, f => f.Name.LastName())
            .RuleFor(a => a.Age, f => f.Random.Byte())
            .RuleFor(a => a.Owner, ownerDtoFaker)
            .RuleFor(a => a.OwnerId, (f, u) => u.Owner!.Id);

        Id = Guid.NewGuid();

        ownerFaker
            .RuleFor(o => o.Id, Id)
            .RuleFor(o => o.FirstName, f => f.Name.FirstName())
            .RuleFor(o => o.LastName, f => f.Name.LastName())
            .RuleFor(o => o.Age, f => f.Random.Byte())
            .RuleFor(o => o.Email, f => f.Internet.Email())
            .RuleFor(o => o.PhoneNumber, f => f.Phone.PhoneNumber())
            .RuleFor(o => o.Animals, animalFaker.Generate(OwnersCount));

        ownerDtoFaker
            .RuleFor(o => o.Id, Id)
            .RuleFor(o => o.FirstName, f => f.Name.FirstName())
            .RuleFor(o => o.LastName, f => f.Name.LastName())
            .RuleFor(o => o.Age, f => f.Random.Byte())
            .RuleFor(o => o.Email, f => f.Internet.Email())
            .RuleFor(o => o.PhoneNumber, f => f.Phone.PhoneNumber())
            .RuleFor(o => o.Animals, animalDtoFaker.Generate(OwnersCount));

        Session = fixture.Freeze<Mock<IMapperSession<Owner>>>();
        TransactionRunner = fixture.Freeze<Mock<TransactionRunner>>();
        Mapper = new MapperConfiguration(options =>
        {
            options.AddProfiles(new Profile[]
            {
                new OwnerProfile(),
                new AnimalProfile()
            });
        }).CreateMapper();
        Logger = fixture.Freeze<Mock<ILogger<OwnersService>>>();

        OwnersService = new OwnersService(
            Session.Object,
            TransactionRunner.Object,
            Mapper,
            Logger.Object);

        OwnersCount = Random.Shared.Next(2, 20);
        Owner = ownerFaker.Generate();
        OwnerDto = ownerDtoFaker.Generate();
        GetAllOwnersQuery = ownerFaker.Generate(OwnersCount).AsQueryable();
        GetByIdOwnersQuery = ownerFaker.Generate(1).AsQueryable();
        GetByIdEmptyOwnersQuery = ownerFaker.Generate(0).AsQueryable();
    }

    public OwnersService OwnersService { get; }
    public Mock<IMapperSession<Owner>> Session { get; }
    public Mock<TransactionRunner> TransactionRunner { get; }
    public Mock<ILogger<OwnersService>> Logger { get; }
    public IMapper Mapper { get; }

    public Guid Id { get; }
    public int OwnersCount { get; }
    public Owner Owner { get; }
    public OwnerDto OwnerDto { get; }
    public IQueryable<Owner> GetAllOwnersQuery { get; }
    public IQueryable<Owner> GetByIdOwnersQuery { get; }
    public IQueryable<Owner> GetByIdEmptyOwnersQuery { get; }
}
