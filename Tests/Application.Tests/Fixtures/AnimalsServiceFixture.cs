using Application.Helpers.Interfaces;
using Application.Services.Implementations;
using Application.Validators.Animal;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Fixtures;

public class AnimalsServiceFixture
{
	public Mock<IMapperSession<Animal>> AnimalSession { get; } = new();
	public Mock<IMapperSession<Owner>> OwnerSession { get; } = new();
	public Mock<ITransactionRunner> TransactionRunner { get; } = new();
	public Mock<ILogger<AnimalsService>> Logger { get; } = new();

	public AnimalsService CreateSut()
	{
		AnimalsService animalsService = new(
			AnimalSession.Object,
			OwnerSession.Object,
			TransactionRunner.Object,
			new AnimalUpdateDtoValidator(),
			Logger.Object);

		return animalsService;
	}
}
