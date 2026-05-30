using Application.DTOs.Animal;
using Application.Exceptions;
using Application.Tests.Fixtures;
using Domain.Entities;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Application.Tests;

[TestClass]
public class AnimalsServiceTests
{
	private AnimalsServiceFixture _fixture = default!;

	[TestInitialize]
	public void SetUp()
	{
		_fixture = new AnimalsServiceFixture();
	}

	[TestMethod]
	public void GetAll_Should_ReturnIQueryableOfAnimalDto()
	{
		// Arrange
		var firstAnimalId = Guid.NewGuid();
		var secondAnimalId = Guid.NewGuid();
		var ownerId = Guid.NewGuid();

		var owner = new Owner
		{
			Id = ownerId,
			FirstName = "Ren",
			LastName = "Amamiya",
			Age = 16,
			Email = "joker@mail.com",
			PhoneNumber = "0983471892",
			Animals = []
		};

		var animalsQuery = new Animal[]
		{
			new()
			{
				Id = firstAnimalId,
				PetName = "Morgana",
				Kind = "Not a cat",
				Age = 1,
				Owner = owner
			},
			new()
			{
				Id = secondAnimalId,
				PetName = "Sissel",
				Kind = "Cat",
				Age = 4,
				Owner = owner
			}
		}.AsQueryable();

		_fixture.AnimalSession
			.Setup(s => s.GetAll())
			.Returns(animalsQuery);

		// Act
		var sut = _fixture.CreateSut();
		var result = sut.GetAll().ToList();

		// Assert
		Assert.AreEqual(2, result.Count);
		Assert.IsTrue(result.All(a => a.Owner.Id == ownerId));

		CollectionAssert.AreEquivalent(
			new[] { firstAnimalId, secondAnimalId},
			result.Select(a => a.Id).ToArray());
	}

	[TestMethod]
	public void GetById_Should_ReturnIQueryableOfAnimalDto()
	{
		// Arrange
		var animalid = Guid.NewGuid();
		var ownerId = Guid.NewGuid();

		var animalQuery = new Animal[]
		{
			new()
			{
				Id = animalid,
				PetName = "Morgana",
				Kind = "Not a cat",
				Age = 1,
				Owner = new Owner
				{
					Id = ownerId,
					FirstName = "Ren",
					LastName = "Amamiya",
					Age = 16,
					Email = "joker@mail.com",
					PhoneNumber = "0983471892",
					Animals = []
				}
			}
		}.AsQueryable();

		_fixture.AnimalSession
			.Setup(s => s.GetById(animalid))
			.Returns(animalQuery);

		// Act
		var sut = _fixture.CreateSut();
		var animal = sut.GetById(animalid).First();

		// Assert
		Assert.IsTrue(animal is not null);
		Assert.AreEqual(animalid, animal.Id);
		Assert.AreEqual(ownerId, animal.Owner.Id);
	}

	[TestMethod]
	public async Task CreateAsync_Should_ReturnAnimalDto_WhenAnimalDtoIsValid()
	{
		// Arrange
		var ownerId = Guid.NewGuid();

		var ownerQuery = new Owner[]
		{
			new()
			{
				Id = ownerId,
				FirstName = "Ren",
				LastName = "Amamiya",
				Age = 16,
				Email = "joker@mail.com",
				PhoneNumber = "0983471892",
				Animals = []
			}
		}.AsQueryable();

		var animalDto = new AnimalCreateDto
		{
			PetName = "Morgana",
			Kind = "Not a cat",
			Age = 1,
			OwnerId = ownerId
		};

		_fixture.OwnerSession
			.Setup(c => c.GetById(ownerId))
			.Returns(ownerQuery);

		// Act
		var sut = _fixture.CreateSut();
		var result = await sut.CreateAsync(animalDto);

		// Assert
		Assert.IsNotNull(result);
		Assert.AreEqual("Morgana", result.PetName);
		Assert.AreEqual(ownerId, result.Owner.Id);
	}

	[TestMethod]
	[ExpectedException(typeof(NullReferenceException))]
	public async Task CreateAsync_Should_ThrowNullReferenceException_WhenOwnerDoesNotExist()
	{
		// Arrange
		var ownerId = Guid.NewGuid();

		var ownerQuery = new Owner[]
		{
			new()
			{
				Id = ownerId,
				FirstName = "Ren",
				LastName = "Amamiya",
				Age = 16,
				Email = "joker@mail.com",
				PhoneNumber = "0983471892",
				Animals = []
			}
		}.AsQueryable();

		var animalDto = new AnimalCreateDto
		{
			PetName = "Morgana",
			Kind = "Not a cat",
			Age = 1,
			OwnerId = ownerId
		};

		_fixture.OwnerSession
			.Setup(c => c.GetById(Guid.NewGuid()))
			.Returns(ownerQuery);

		// Act
		var sut = _fixture.CreateSut();
		await sut.CreateAsync(animalDto);
	}

	[TestMethod]
	public async Task UpdateAsync_Should_ReturnTask_WhenAnimalDtoIsValid()
	{
		// Arrange
		var animalId = Guid.NewGuid();
		var ownerId = Guid.NewGuid();

		var owner = new Owner()
		{
			Id = ownerId,
			FirstName = "Ren",
			LastName = "Amamiya",
			Age = 16,
			Email = "joker@mail.com",
			PhoneNumber = "0983471892",
			Animals = []
		};

		var ownerQuery = new Owner[] { owner }.AsQueryable();

		var animalQuery = new Animal[]
		{
			new()
			{
				PetName = "Fluffy",
				Kind = "Cat",
				Age = 2,
				Owner = owner
			}
		}.AsQueryable();

		var animalDto = new AnimalUpdateDto
		{
			PetName = "Morgana",
			Kind = "Not a cat",
			Age = 1,
			OwnerId = ownerId
		};

		_fixture.OwnerSession
			.Setup(c => c.GetById(ownerId))
			.Returns(ownerQuery);

		_fixture.AnimalSession
			.Setup(c => c.GetById(animalId))
			.Returns(animalQuery);

		try
		{
			// Act
			var sut = _fixture.CreateSut();
			await sut.UpdateAsync(animalId, animalDto);
		}
		catch
		{
			// Assert
			Assert.Fail();
		}
	}

	[TestMethod]
	[ExpectedException(typeof(NullReferenceException))]
	public async Task UpdateAsync_Should_ThrowNullReferenceException_WhenOwnerDoesNotExist()
	{
		// Arrange
		var ownerId = Guid.NewGuid();

		var ownerQuery = new Owner[]
		{
			new()
			{
				Id = ownerId,
				FirstName = "Ren",
				LastName = "Amamiya",
				Age = 16,
				Email = "joker@mail.com",
				PhoneNumber = "0983471892",
				Animals = []
			}
		}.AsQueryable();

		var animalDto = new AnimalUpdateDto
		{
			PetName = "Morgana",
			Kind = "Not a cat",
			Age = 1,
			OwnerId = Guid.NewGuid()
		};

		_fixture.OwnerSession
			.Setup(s => s.GetById(ownerId))
			.Returns(ownerQuery);

		// Act
		var sut = _fixture.CreateSut();
		await sut.UpdateAsync(Guid.NewGuid(), animalDto);
	}

	[TestMethod]
	[ExpectedException(typeof(NullReferenceException))]
	public async Task UpdateAsync_Should_ThrowNullReferenceException_WhenAnimalDoesNotExist()
	{
		// Arrange
		var animalId = Guid.NewGuid();
		var ownerId = Guid.NewGuid();

		var owner = new Owner()
		{
			Id = ownerId,
			FirstName = "Ren",
			LastName = "Amamiya",
			Age = 16,
			Email = "joker@mail.com",
			PhoneNumber = "0983471892",
			Animals = []
		};

		var ownerQuery = new Owner[] { owner }.AsQueryable();

		var animalQuery = new Animal[]
		{
			new()
			{
				Id = animalId,
				PetName = "Morgana",
				Kind = "Not a cat",
				Age = 1,
				Owner = owner
			}
		}.AsQueryable();

		var animalDto = new AnimalUpdateDto
		{
			PetName = "Morgana",
			Kind = "Not a cat",
			Age = 1,
			OwnerId = ownerId
		};

		_fixture.OwnerSession
			.Setup(c => c.GetById(ownerId))
			.Returns(ownerQuery);

		_fixture.AnimalSession
			.Setup(c => c.GetById(animalId))
			.Returns(animalQuery);

		// Act
		var sut = _fixture.CreateSut();
		await sut.UpdateAsync(Guid.NewGuid(), animalDto);
	}

	[TestMethod]
	[DataRow(nameof(AnimalUpdateDto.PetName), "Sissel")]
	[DataRow(nameof(AnimalUpdateDto.Kind), "Definitely, not a cat")]
	[DataRow(nameof(AnimalUpdateDto.Age), 3)]
	public async Task UpdateAsync_Should_ReturnTask_WhenAnimalDeltaIsValid(string property, object value)
	{
		// Arrange
		var animalId = Guid.NewGuid();
		var ownerId = Guid.NewGuid();

		var owner = new Owner
		{
			Id = ownerId,
			FirstName = "Ren",
			LastName = "Amamiya",
			Age = 16,
			Email = "joker@mail.com",
			PhoneNumber = "0983471892",
			Animals = []
		};

		var ownerQuery = new Owner[] { owner }.AsQueryable();

		var animalQuery = new Animal[]
		{
			new()
			{
				PetName = "Fluffy",
				Kind = "Cat",
				Age = 2,
				Owner = owner
			}
		}.AsQueryable();

		var animalDto = new AnimalUpdateDto
		{
			PetName = "Morgana",
			Kind = "Not a cat",
			Age = 1,
			OwnerId = ownerId
		};

		var delta = new Delta<AnimalUpdateDto>();
		delta.TrySetPropertyValue(property, value);

		_fixture.OwnerSession
			.Setup(c => c.GetById(ownerId))
			.Returns(ownerQuery);

		_fixture.AnimalSession
			.Setup(c => c.GetById(animalId))
			.Returns(animalQuery);

		try
		{
			// Act
			var sut = _fixture.CreateSut();
			await sut.UpdateAsync(animalId, delta);
		}
		catch
		{
			// Assert
			Assert.Fail();
		}
	}

	[TestMethod]
	[DynamicData(nameof(GetPatchValidationExceptionData), DynamicDataSourceType.Method)]
	[ExpectedException(typeof(ValidationException))]
	public async Task UpdateAsync_Should_ThrowValidationException_WhenAnimalDeltaIsInvalid(
		string property,
		object value)
	{
		// Arrange
		var animalId = Guid.NewGuid();
		var ownerId = Guid.NewGuid();

		var owner = new Owner
		{
			Id = ownerId,
			FirstName = "Ren",
			LastName = "Amamiya",
			Age = 16,
			Email = "joker@mail.com",
			PhoneNumber = "0983471892",
			Animals = []
		};

		var ownerQuery = new Owner[] { owner }.AsQueryable();

		var animalQuery = new Animal[]
		{
			new()
			{
				PetName = "Fluffy",
				Kind = "Cat",
				Age = 2,
				Owner = owner
			}
		}.AsQueryable();

		var animalDto = new AnimalUpdateDto
		{
			PetName = "Morgana",
			Kind = "Not a cat",
			Age = 1,
			OwnerId = ownerId
		};

		var delta = new Delta<AnimalUpdateDto>();
		delta.TrySetPropertyValue(property, value);

		_fixture.OwnerSession
			.Setup(c => c.GetById(ownerId))
			.Returns(ownerQuery);

		_fixture.AnimalSession
			.Setup(c => c.GetById(animalId))
			.Returns(animalQuery);

		// Act
		var sut = _fixture.CreateSut();
		await sut.UpdateAsync(animalId, delta);
	}

	[TestMethod]
	[ExpectedException(typeof(NullReferenceException))]
	public async Task UpdateAsync_Should_ThrowNullReferenceException_WithDeltaWhenAnimalDoesNotExist()
	{
		// Arrange
		var ownerId = Guid.NewGuid();

		var ownerQuery = new Owner[]
		{
			new()
			{
				Id = ownerId,
				FirstName = "Ren",
				LastName = "Amamiya",
				Age = 16,
				Email = "joker@mail.com",
				PhoneNumber = "0983471892",
				Animals = []
			}
		}.AsQueryable();

		var delta = new Delta<AnimalUpdateDto>();
		delta.TrySetPropertyValue(nameof(AnimalUpdateDto.OwnerId), Guid.NewGuid());
		delta.TrySetPropertyValue(nameof(AnimalUpdateDto.PetName), "Kitty");

		_fixture.OwnerSession
			.Setup(s => s.GetById(ownerId))
			.Returns(ownerQuery);

		// Act
		var sut = _fixture.CreateSut();
		await sut.UpdateAsync(Guid.NewGuid(), new Delta<AnimalUpdateDto>());
	}

	[TestMethod]
	[ExpectedException(typeof(NullReferenceException))]
	public async Task UpdateAsync_Should_ThrowNullReferenceException_WithDeltaWhenOwnerDoesNotExist()
	{
		// Arrange
		var animalId = Guid.NewGuid();
		var ownerId = Guid.NewGuid();

		var owner = new Owner()
		{
			Id = ownerId,
			FirstName = "Ren",
			LastName = "Amamiya",
			Age = 16,
			Email = "joker@mail.com",
			PhoneNumber = "0983471892",
			Animals = []
		};

		var ownerQuery = new Owner[] { owner }.AsQueryable();

		var animalQuery = new Animal[]
		{
			new()
			{
				Id = animalId,
				PetName = "Morgana",
				Kind = "Not a cat",
				Age = 1,
				Owner = owner
			}
		}.AsQueryable();

		var delta = new Delta<AnimalUpdateDto>();
		delta.TrySetPropertyValue(nameof(AnimalUpdateDto.Age), 3);
		delta.TrySetPropertyValue(nameof(AnimalUpdateDto.Kind), "Definitely, not a cat");

		_fixture.OwnerSession
			.Setup(c => c.GetById(ownerId))
			.Returns(ownerQuery);

		_fixture.AnimalSession
			.Setup(c => c.GetById(animalId))
			.Returns(animalQuery);

		// Act
		var sut = _fixture.CreateSut();
		await sut.UpdateAsync(Guid.NewGuid(), delta);
	}

	[TestMethod]
	public async Task DeleteAsync_Should_ReturnTask_WhenAnimalDtoIsValid()
	{
		// Arrange
		var animalId = Guid.NewGuid();

		var animalQuery = new Animal[]
		{
			new()
			{
				Id = animalId,
				PetName = "Morgana",
				Kind = "Not a cat",
				Age = 1,
				Owner = new()
				{
					Id = Guid.NewGuid(),
					FirstName = "Ren",
					LastName = "Amamiya",
					Age = 16,
					Email = "joker@mail.com",
					PhoneNumber = "0983471892",
					Animals = []
				}
			}
		}.AsQueryable();

		_fixture.AnimalSession
			.Setup(c => c.GetById(animalId))
			.Returns(animalQuery);

		try
		{
			// Act
			var sut = _fixture.CreateSut();
			await sut.DeleteAsync(animalId);
		}
		catch
		{
			// Assert
			Assert.Fail();
		}
	}

	[TestMethod]
	[ExpectedException(typeof(NullReferenceException))]
	public async Task DeleteAsync_Should_ThrowNullReferenceException_WhenAnimalDoesNotExist()
	{
		// Arrange
		var animalId = Guid.NewGuid();

		var animalQuery = new Animal[]
		{
			new()
			{
				Id = animalId,
				PetName = "Morgana",
				Kind = "Not a cat",
				Age = 1,
				Owner = new()
				{
					Id = Guid.NewGuid(),
					FirstName = "Ren",
					LastName = "Amamiya",
					Age = 16,
					Email = "joker@mail.com",
					PhoneNumber = "0983471892",
					Animals = []
				}
			}
		}.AsQueryable();

		_fixture.AnimalSession
			.Setup(c => c.GetById(animalId))
			.Returns(animalQuery);

		// Act
		var sut = _fixture.CreateSut();
		await sut.DeleteAsync(Guid.NewGuid());
	}

	private static IEnumerable<object[]> GetPatchValidationExceptionData()
	{
		yield return new object[]
		{
			nameof(AnimalUpdateDto.PetName),
			string.Empty
		};

		yield return new object[]
		{
			nameof(AnimalUpdateDto.PetName),
			"New pet name exceeding length"
		};

		yield return new object[]
		{
			nameof(AnimalUpdateDto.Kind),
			string.Empty
		};

		yield return new object[]
		{
			nameof(AnimalUpdateDto.Kind),
			"New long pet kind that exceeds max length validation"
		};

		yield return new object[]
		{
			nameof(AnimalUpdateDto.Age),
			(byte)0
		};

		yield return new object[]
		{
			nameof(AnimalUpdateDto.Age),
			(byte)51
		};
	}
}
