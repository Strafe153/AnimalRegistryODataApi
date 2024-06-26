﻿using AnimalRegistryODataApi.Controllers;
using Application.DTOs;
using Application.Services.Interfaces;
using AutoFixture;
using AutoFixture.AutoMoq;
using Bogus;
using Microsoft.AspNetCore.OData.Deltas;
using Moq;

namespace Api.Tests.Fixtures;

public class OwnersControllerFixture
{
	public OwnersControllerFixture()
	{
		var fixture = new Fixture().Customize(new AutoMoqCustomization());

		var ownerDtoFaker = new Faker<OwnerDto>();

		var animalDtoFaker = new Faker<AnimalDto>()
			.RuleFor(a => a.Id, Guid.NewGuid())
			.RuleFor(a => a.PetName, f => f.Name.FirstName())
			.RuleFor(a => a.Kind, f => f.Name.LastName())
			.RuleFor(a => a.Age, f => f.Random.Byte())
			.RuleFor(a => a.Owner, ownerDtoFaker)
			.RuleFor(a => a.OwnerId, (f, u) => u.Owner!.Id);

		ownerDtoFaker
			.RuleFor(o => o.Id, Id)
			.RuleFor(o => o.FirstName, f => f.Name.FirstName())
			.RuleFor(o => o.LastName, f => f.Name.LastName())
			.RuleFor(o => o.Age, f => f.Random.Byte())
			.RuleFor(o => o.Email, f => f.Internet.Email())
			.RuleFor(o => o.PhoneNumber, f => f.Phone.PhoneNumber())
			.RuleFor(o => o.Animals, animalDtoFaker.Generate(OwnerDtosCount));

		OwnersService = fixture.Freeze<Mock<IOwnersService>>();

		OwnersController = new OwnersController(OwnersService.Object);

		Id = Guid.NewGuid();
		OwnerDtosCount = Random.Shared.Next(2, 20);
		OwnerDto = ownerDtoFaker.Generate();
		OwnerDtoDelta = new Delta<OwnerDto>();
		AnimalDto = animalDtoFaker.Generate();
		OwnerDtos = ownerDtoFaker.Generate(OwnerDtosCount);
		OwnerDtoQuery = OwnerDtos.AsQueryable();
	}

	public OwnersController OwnersController { get; }
	public Mock<IOwnersService> OwnersService { get; }

	public Guid Id { get; }
	public int OwnerDtosCount { get; }
	public OwnerDto OwnerDto { get; }
	public Delta<OwnerDto> OwnerDtoDelta { get; }
	public AnimalDto AnimalDto { get; }
	public IList<OwnerDto> OwnerDtos { get; }
	public IQueryable<OwnerDto> OwnerDtoQuery { get; }
}
