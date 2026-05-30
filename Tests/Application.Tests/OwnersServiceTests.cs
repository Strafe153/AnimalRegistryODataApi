using Application.DTOs.Owner;
using Application.Exceptions;
using Application.Tests.Fixtures;
using Domain.Entities;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Application.Tests;

[TestClass]
public class OwnersServiceTests
{
	private OwnersServiceFixture _fixture = default!;

	[TestInitialize]
	public void SetUp()
	{
		_fixture = new OwnersServiceFixture();
	}

	[TestMethod]
	public void GetAll_Should_ReturnIQueryableOfOwnerDto()
	{
		// Arrange
		var firstOwnerId = Guid.NewGuid();
		var secondOwnerId = Guid.NewGuid();
		
		var firstOwner = new Owner
		{
			Id = firstOwnerId,
			FirstName = "Ren",
			LastName = "Amamiya",
			Age = 16,
			Email = "joker@mail.com",
			PhoneNumber = "0983471892",
			Animals = [
				new()
				{
					Id = Guid.NewGuid(),
					PetName = "Morgana",
					Kind = "Not a cat",
					Age = 1
				}
			]
		};

		var secondOwner = new Owner
		{
			Id = secondOwnerId,
			FirstName = "Goro",
			LastName = "Akechi",
			Age = 17,
			Email = "crow@mail.com",
			PhoneNumber = "0953876841",
			Animals = [
				new()
				{
					Id = Guid.NewGuid(),
					PetName = "Doggo",
					Kind = "Husky",
					Age = 3
				}
			]
		};

		var ownersQuery = new Owner[] { firstOwner, secondOwner }.AsQueryable();

		_fixture.OwnerSession
			.Setup(s => s.GetAll())
			.Returns(ownersQuery);

		// Act
		var sut = _fixture.CreateSut();
		var result = sut.GetAll().ToList();

		// Assert
		Assert.AreEqual(2, result.Count);
		Assert.IsTrue(result.All(o => o.Animals.Count == 1));

		CollectionAssert.AreEquivalent(
			new[] { firstOwnerId, secondOwnerId},
			result.Select(a => a.Id).ToArray());
	}

	[TestMethod]
	public void GetById_Should_ReturnIQueryableOfOwnerDto()
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
				Animals = [
					new()
					{
						Id = Guid.NewGuid(),
						PetName = "Morgana",
						Kind = "Not a cat",
						Age = 1
					}
				]
			}
		}.AsQueryable();

		_fixture.OwnerSession
			.Setup(s => s.GetById(ownerId))
			.Returns(ownerQuery);

		// Act
		var sut = _fixture.CreateSut();
		var owner = sut.GetById(ownerId).First();

		// Assert
		Assert.IsTrue(owner is not null);
		Assert.AreEqual(ownerId, owner.Id);
		Assert.IsTrue(owner.Animals.Count > 0);
	}

	[TestMethod]
	public async Task CreateAsync_Should_ReturnOwnerDto_WhenOwnerDtoIsValid()
	{
		// Arrange
		var ownerDto = new OwnerCreateDto
		{
			FirstName = "Ren",
			LastName = "Amamiya",
			Age = 16,
			Email = "joker@mail.com",
			PhoneNumber = "0983471892"
		};

		// Act
		var sut = _fixture.CreateSut();
		var owner = await sut.CreateAsync(ownerDto);

		// Assert
		Assert.IsNotNull(owner);
		Assert.AreEqual("Ren", owner.FirstName);
		Assert.AreEqual(16, owner.Age);
	}

	[TestMethod]
	public async Task UpdateAsync_Should_ReturnTask_WhenOwnerDtoIsValid()
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
				Animals = [
					new()
					{
						Id = Guid.NewGuid(),
						PetName = "Morgana",
						Kind = "Not a cat",
						Age = 1
					}
				]
			}
		}.AsQueryable();

		var ownerDto = new OwnerUpdateDto
		{
			FirstName = "Goro",
			LastName = "Akechi",
			Age = 17,
			Email = "crow@mail.com",
			PhoneNumber = "0953876841",
		};

		_fixture.OwnerSession
			.Setup(c => c.GetById(ownerId))
			.Returns(ownerQuery);

		try
		{
			// Act
			var sut = _fixture.CreateSut();
			await sut.UpdateAsync(ownerId, ownerDto);
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
				Animals = [
					new()
					{
						Id = Guid.NewGuid(),
						PetName = "Morgana",
						Kind = "Not a cat",
						Age = 1
					}
				]
			}
		}.AsQueryable();

		var ownerDto = new OwnerUpdateDto
		{
			FirstName = "Goro",
			LastName = "Akechi",
			Age = 17,
			Email = "crow@mail.com",
			PhoneNumber = "0953876841",
		};

		_fixture.OwnerSession
			.Setup(s => s.GetById(ownerId))
			.Returns(ownerQuery);

		// Act
		var sut = _fixture.CreateSut();
		await sut.UpdateAsync(Guid.NewGuid(), ownerDto);
	}

	[TestMethod]
	[DynamicData(nameof(GetPatchValidData), DynamicDataSourceType.Method)]
	public async Task UpdateAsync_Should_ReturnTask_WhenOwnerDeltaIsValid(string property, object value)
	{
		// Arrange
		var ownerId = Guid.NewGuid();

		var owner = new Owner()
		{
			Id = ownerId,
			FirstName = "Ren",
			LastName = "Amamiya",
			Age = 16,
			Email = "joker@mail.com",
			PhoneNumber = "0983471892",
			Animals = [
				new()
				{
					Id = Guid.NewGuid(),
					PetName = "Morgana",
					Kind = "Not a cat",
					Age = 1
				}
			]
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

		var delta = new Delta<OwnerUpdateDto>();
		delta.TrySetPropertyValue(property, value);

		_fixture.OwnerSession
			.Setup(c => c.GetById(ownerId))
			.Returns(ownerQuery);

		try
		{
			// Act
			var sut = _fixture.CreateSut();
			await sut.UpdateAsync(ownerId, delta);
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
	public async Task UpdateAsync_Should_ThrowValidationException_WhenOwnerDeltaIsInvalid(
		string property,
		object value)
	{
		// Arrange
		var ownerId = Guid.NewGuid();

		var owner = new Owner()
		{
			Id = ownerId,
			FirstName = "Ren",
			LastName = "Amamiya",
			Age = 16,
			Email = "joker@mail.com",
			PhoneNumber = "0983471892",
			Animals = [
				new()
				{
					Id = Guid.NewGuid(),
					PetName = "Morgana",
					Kind = "Not a cat",
					Age = 1
				}
			]
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

		var delta = new Delta<OwnerUpdateDto>();
		delta.TrySetPropertyValue(property, value);

		_fixture.OwnerSession
			.Setup(c => c.GetById(ownerId))
			.Returns(ownerQuery);

		// Act
		var sut = _fixture.CreateSut();
		await sut.UpdateAsync(ownerId, delta);
	}

	[TestMethod]
	[ExpectedException(typeof(NullReferenceException))]
	public async Task UpdateAsync_Should_ThrowNullReferenceException_WithDeltaWhenOwnerDoesNotExist()
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
				Animals = [
					new()
					{
						Id = Guid.NewGuid(),
						PetName = "Morgana",
						Kind = "Not a cat",
						Age = 1
					}
				]
			}
		}.AsQueryable();

		var delta = new Delta<OwnerUpdateDto>();
		delta.TrySetPropertyValue(nameof(OwnerUpdateDto.FirstName), "Goro");
		delta.TrySetPropertyValue(nameof(OwnerUpdateDto.Email), "crow@mail.com");

		_fixture.OwnerSession
			.Setup(s => s.GetById(ownerId))
			.Returns(ownerQuery);

		// Act
		var sut = _fixture.CreateSut();
		await sut.UpdateAsync(Guid.NewGuid(), delta);
	}

	[TestMethod]
	public async Task DeleteAsync_Should_ReturnTask_WhenOwnerDtoIsValid()
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
				Animals = [
					new()
					{
						Id = Guid.NewGuid(),
						PetName = "Morgana",
						Kind = "Not a cat",
						Age = 1
					}
				]
			}
		}.AsQueryable();

		_fixture.OwnerSession
			.Setup(c => c.GetById(ownerId))
			.Returns(ownerQuery);

		try
		{
			// Act
			var sut = _fixture.CreateSut();
			await sut.DeleteAsync(ownerId);
		}
		catch
		{
			// Assert
			Assert.Fail();
		}
	}

	[TestMethod]
	[ExpectedException(typeof(NullReferenceException))]
	public async Task DeleteAsync_Should_ThrowNullReferenceException_WhenOwnerDoesNotExist()
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
				Animals = [
					new()
					{
						Id = Guid.NewGuid(),
						PetName = "Morgana",
						Kind = "Not a cat",
						Age = 1
					}
				]
			}
		}.AsQueryable();

		_fixture.OwnerSession
			.Setup(c => c.GetById(ownerId))
			.Returns(ownerQuery);

		// Act
		var sut = _fixture.CreateSut();
		await sut.DeleteAsync(Guid.NewGuid());
	}

	private static IEnumerable<object[]> GetPatchValidData()
	{
		yield return new object[]
		{
			nameof(OwnerUpdateDto.FirstName),
			"Goro"
		};

		yield return new object[]
		{
			nameof(OwnerUpdateDto.LastName),
			"Akechi"
		};

		yield return new object[]
		{
			nameof(OwnerUpdateDto.Age),
			(byte)17
		};

		yield return new object[]
		{
			nameof(OwnerUpdateDto.Email),
			"crow@mail.com"
		};

		yield return new object[]
		{
			nameof(OwnerUpdateDto.PhoneNumber),
			"0953876841"
		};
	}

	private static IEnumerable<object[]> GetPatchValidationExceptionData()
	{
		yield return new object[]
		{
			nameof(OwnerUpdateDto.FirstName),
			"A"
		};

		yield return new object[]
		{
			nameof(OwnerUpdateDto.FirstName),
			"Long first name exceeding max length"
		};

		yield return new object[]
		{
			nameof(OwnerUpdateDto.LastName),
			"B"
		};

		yield return new object[]
		{
			nameof(OwnerUpdateDto.LastName),
			"Long last name exceeding max length"
		};

		yield return new object[]
		{
			nameof(OwnerUpdateDto.Age),
			(byte)13
		};

		yield return new object[]
		{
			nameof(OwnerUpdateDto.Age),
			(byte)101
		};

		yield return new object[]
		{
			nameof(OwnerUpdateDto.Email),
			"not_an_email"
		};

		yield return new object[]
		{
			nameof(OwnerUpdateDto.PhoneNumber),
			"12345678"
		};

		yield return new object[]
		{
			nameof(OwnerUpdateDto.PhoneNumber),
			"987654321012345678901"
		};
	}
}
