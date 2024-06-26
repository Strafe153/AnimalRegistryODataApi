﻿using Application.AutoMapperProfiles;
using Application.DTOs;
using Application.Helpers.Implementations;
using Application.Services.Implementations;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using Bogus;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.OData.Deltas;
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

		AnimalSession = fixture.Freeze<Mock<IMapperSession<Animal>>>();
		OwnerSession = fixture.Freeze<Mock<IMapperSession<Owner>>>();
		TransactionRunner = fixture.Freeze<Mock<TransactionRunner>>();
		Mapper = new MapperConfiguration(options =>
		{
			options.AddProfiles([
				new AnimalProfile(),
				new OwnerProfile()
			]);
		}).CreateMapper();
		Logger = fixture.Freeze<Mock<ILogger<AnimalsService>>>();

		AnimalsService = new AnimalsService(
			AnimalSession.Object,
			OwnerSession.Object,
			TransactionRunner.Object,
			Mapper,
			Logger.Object);

		AnimalsCount = Random.Shared.Next(2, 20);
		AnimalDto = animalDtoFaker.Generate();
		AnimalDtoDelta = new Delta<AnimalDto>();
		GetAllAnimalsQuery = animalFaker.Generate(AnimalsCount).AsQueryable();
		GetByIdAnimalsQuery = animalFaker.Generate(1).AsQueryable();
		GetByIdEmptyAnimalsQuery = animalFaker.Generate(0).AsQueryable();
		GetByIdOwnersQuery = ownerFaker.Generate(1).AsQueryable();
		GetByIdEmptyOwnersQuery = ownerFaker.Generate(0).AsQueryable();
	}

	public AnimalsService AnimalsService { get; }
	public Mock<IMapperSession<Animal>> AnimalSession { get; }
	public Mock<IMapperSession<Owner>> OwnerSession { get; }
	public Mock<TransactionRunner> TransactionRunner { get; }
	public Mock<ILogger<AnimalsService>> Logger { get; }
	public IMapper Mapper { get; }

	public Guid Id { get; }
	public int AnimalsCount { get; }
	public AnimalDto AnimalDto { get; }
	public Delta<AnimalDto> AnimalDtoDelta { get; }
	public IQueryable<Animal> GetAllAnimalsQuery { get; }
	public IQueryable<Animal> GetByIdAnimalsQuery { get; }
	public IQueryable<Animal> GetByIdEmptyAnimalsQuery { get; }
	public IQueryable<Owner> GetByIdOwnersQuery { get; }
	public IQueryable<Owner> GetByIdEmptyOwnersQuery { get; }
}
