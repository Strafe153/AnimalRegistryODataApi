using Application.Helpers.Interfaces;
using Application.Services.Implementations;
using Application.Validators.Owner;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Fixtures;

public class OwnersServiceFixture
{
	public Mock<IMapperSession<Owner>> OwnerSession { get; } = new();
	public Mock<ITransactionRunner> TransactionRunner { get; } = new();
	public Mock<ILogger<OwnersService>> Logger { get; } = new();

	public OwnersService CreateSut()
	{
		OwnersService animalsService = new(
			OwnerSession.Object,
			TransactionRunner.Object,
			new OwnerUpdateDtoValidator(),
			Logger.Object);

		return animalsService;
	}
}
