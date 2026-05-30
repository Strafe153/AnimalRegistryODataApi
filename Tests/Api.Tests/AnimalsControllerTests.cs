using Api.Tests.Fixtures;
using Application.DTOs.Animal;
using Application.DTOs.Owner;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Api.Tests;

[TestClass]
public class AnimalsControllerTests
{
	private AnimalsControllerFixture _fixture = default!;

	[TestInitialize]
	public void SetUp()
	{
		_fixture = new AnimalsControllerFixture();
	}

	[TestMethod]
	public void Get_Should_ReturnIQueryableOfAnimalDto()
	{
		var owner = new OwnerReadDto
		{
			Id = Guid.NewGuid(),
			FirstName = "Ren",
			LastName = "Amamiya",
			Age = 16,
			Email = "joker@mail.com",
			PhoneNumber = "0983471892",
			Animals = []
		};

		var animalsQuery = new List<AnimalReadDto>
		{
			new()
			{
				Id = Guid.NewGuid(),
				PetName = "Morgana",
				Kind = "Not a cat",
				Age = 1,
				Owner = owner
			},
			new()
			{
				Id = Guid.NewGuid(),
				PetName = "Sissel",
				Kind = "Cat",
				Age = 4,
				Owner = owner
			}
		}.AsQueryable();

		_fixture.AnimalsService
			.Setup(s => s.GetAll())
			.Returns(animalsQuery);

		// Act
		var sut = _fixture.CreateSut();
		var result = sut.Get();
		var objectResult = result.Result as OkObjectResult;
		var queryResult = objectResult?.Value as IQueryable<AnimalReadDto>;

		// Assert
		Assert.IsInstanceOfType<ActionResult<IQueryable<AnimalReadDto>>>(result);
		Assert.AreEqual(StatusCodes.Status200OK, objectResult?.StatusCode);
		Assert.IsNotNull(queryResult);
		Assert.AreEqual(2, queryResult.Count());
	}

	[TestMethod]
	public void Get_Should_ReturnSingleResultOfAnimalDto_WhenAnimalExists()
	{
		// Arrange
		var animalId = Guid.NewGuid();

		var animalQuery = new AnimalReadDto[]
		{
			new()
			{
				PetName = "Morgana",
				Kind = "Not a cat",
				Age = 1,
				Owner = new OwnerReadDto
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

		_fixture.AnimalsService
			.Setup(s => s.GetById(animalId))
			.Returns(animalQuery);

		// Act
		var sut = _fixture.CreateSut();
		var result = sut.Get(animalId);
		var objectResult = result.Result as OkObjectResult;
		var singleResult = objectResult?.Value as SingleResult<AnimalReadDto>;

		// Assert
		Assert.IsNotNull(result);
		Assert.IsInstanceOfType<ActionResult<SingleResult<AnimalReadDto>>>(result);
		Assert.AreEqual(StatusCodes.Status200OK, objectResult?.StatusCode);
		Assert.IsNotNull(singleResult);
	}

	[TestMethod]
	public async Task Post_Should_ReturnActionResultOfAnimalDto_WhenAnimalDtoIsValid()
	{
		// Arrange
		var ownerId = Guid.NewGuid();

		var createDto = new AnimalCreateDto
		{
			PetName = "Morgana",
			Kind = "Not a cat",
			Age = 1,
			OwnerId = ownerId
		};

		var readDto = new AnimalReadDto
		{
			Id = Guid.NewGuid(),
			PetName = "Morgana",
			Kind = "Not a cat",
			Age = 1,
			Owner = new OwnerReadDto
			{
				Id = ownerId,
				FirstName = "Ren",
				LastName = "Amamiya",
				Age = 16,
				Email = "joker@mail.com",
				PhoneNumber = "0983471892",
				Animals = []
			}
		};

		_fixture.AnimalsService
			.Setup(s => s.CreateAsync(createDto))
			.ReturnsAsync(readDto);

		// Act
		var sut = _fixture.CreateSut();
		var result = await sut.Post(createDto);
		var objectResult = result.Result as CreatedAtActionResult;
		var animalDto = objectResult?.Value as AnimalReadDto;

		// Assert
		Assert.IsNotNull(result);
		Assert.IsInstanceOfType<ActionResult<AnimalReadDto>>(result);
		Assert.AreEqual(StatusCodes.Status201Created, objectResult?.StatusCode);
		Assert.IsNotNull(createDto);
	}

	[TestMethod]
	public async Task Put_Should_ReturnNoContentResult_WhenAnimalExists()
	{
		// Arrange
		var updateDto = new AnimalUpdateDto
		{
			PetName = "Morgana",
			Kind = "Not a cat",
			Age = 1,
			OwnerId = Guid.NewGuid()
		};

		// Act
		var sut = _fixture.CreateSut();
		var result = await sut.Put(Guid.NewGuid(), updateDto);
		var objectResult = result as NoContentResult;

		// Assert
		Assert.IsNotNull(result);
		Assert.IsInstanceOfType<NoContentResult>(result);
		Assert.AreEqual(StatusCodes.Status204NoContent, objectResult?.StatusCode);
	}

	[TestMethod]
	public async Task Patch_Should_ReturnNoContentResult_WhenAnimalExists()
	{
		// Arrange
		var delta = new Delta<AnimalUpdateDto>();

		delta.TrySetPropertyValue(nameof(AnimalUpdateDto.PetName), "Morgana");
		delta.TrySetPropertyValue(nameof(AnimalUpdateDto.Age), 2);

		// Act
		var sut = _fixture.CreateSut();
		var result = await sut.Patch(Guid.NewGuid(), delta);
		var objectResult = result as NoContentResult;

		// Assert
		Assert.IsNotNull(result);
		Assert.IsInstanceOfType<NoContentResult>(result);
		Assert.AreEqual(StatusCodes.Status204NoContent, objectResult?.StatusCode);
	}

	[TestMethod]
	public async Task Delete_Should_ReturnNoContentResult_WhenAnimalExists()
	{
		// Act
		var sut = _fixture.CreateSut();
		var result = await sut.Delete(Guid.NewGuid());
		var objectResult = result as NoContentResult;

		// Assert
		Assert.IsNotNull(result);
		Assert.IsInstanceOfType<NoContentResult>(result);
		Assert.AreEqual(StatusCodes.Status204NoContent, objectResult?.StatusCode);
	}
}
