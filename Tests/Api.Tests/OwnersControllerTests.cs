using Api.Tests.Fixtures;
using Application.DTOs.Owner;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Api.Tests;

[TestClass]
public class OwnersControllerTests
{
	private OwnersControllerFixture _fixture = default!;

	[TestInitialize]
	public void SetUp()
	{
		_fixture = new OwnersControllerFixture();
	}

	[TestMethod]
	public void Get_Should_ReturnIQueryableOfOwnerDto()
	{
		// Arrange
		var firstOwner = new OwnerReadDto
		{
			Id = Guid.NewGuid(),
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

		var secondOwner = new OwnerReadDto
		{
			Id = Guid.NewGuid(),
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

		var ownersQuery = new OwnerReadDto[] { firstOwner, secondOwner }.AsQueryable();

		_fixture.OwnersService
			.Setup(s => s.GetAll())
			.Returns(ownersQuery);

		// Act
		var sut = _fixture.CreateSut();
		var result = sut.Get();
		var objectResult = result.Result as OkObjectResult;
		var queryResult = objectResult?.Value as IQueryable<OwnerReadDto>;

		// Assert
		Assert.IsInstanceOfType<ActionResult<IQueryable<OwnerReadDto>>>(result);
		Assert.AreEqual(StatusCodes.Status200OK, objectResult?.StatusCode);
		Assert.IsNotNull(queryResult);
		Assert.AreEqual(2, queryResult.Count());
	}

	[TestMethod]
	public void Get_Should_ReturnSingleResultOfOwnerDto_WhenOwnerExists()
	{
		// Arrange
		var ownerId = Guid.NewGuid();

		var ownerQuery = new OwnerReadDto[]
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


		_fixture.OwnersService
			.Setup(s => s.GetById(ownerId))
			.Returns(ownerQuery);

		// Act
		var sut = _fixture.CreateSut();
		var result = sut.Get(ownerId);
		var objectResult = result.Result as OkObjectResult;
		var singleResult = objectResult?.Value as SingleResult<OwnerReadDto>;

		// Assert
		Assert.IsNotNull(result);
		Assert.IsInstanceOfType<ActionResult<SingleResult<OwnerReadDto>>>(result);
		Assert.AreEqual(StatusCodes.Status200OK, objectResult?.StatusCode);
		Assert.IsNotNull(singleResult);
	}

	[TestMethod]
	public async Task Post_Should_ReturnActionResultOfOwnerDto_WhenOwnerDtoIsValid()
	{
		// Arrange
		var createDto = new OwnerCreateDto
		{
			FirstName = "Ren",
			LastName = "Amamiya",
			Age = 16,
			Email = "joker@mail.com",
			PhoneNumber = "0983471892"
		};

		var readDto = new OwnerReadDto
		{
			Id = Guid.NewGuid(),
			FirstName = "Ren",
			LastName = "Amamiya",
			Age = 16,
			Email = "joker@mail.com",
			PhoneNumber = "0983471892"
		};

		_fixture.OwnersService
			.Setup(s => s.CreateAsync(createDto))
			.ReturnsAsync(readDto);

		// Act
		var sut = _fixture.CreateSut();
		var result = await sut.Post(createDto);
		var objectResult = result.Result as CreatedAtActionResult;
		var ownerDto = objectResult?.Value as OwnerReadDto;

		// Assert
		Assert.IsNotNull(result);
		Assert.IsInstanceOfType<ActionResult<OwnerReadDto>>(result);
		Assert.AreEqual(StatusCodes.Status201Created, objectResult?.StatusCode);
		Assert.IsNotNull(createDto);
	}

	[TestMethod]
	public async Task Put_Should_ReturnNoContentResult_WhenOwnerExists()
	{
		// Arrange
		var updateDto = new OwnerUpdateDto
		{
			FirstName = "Ren",
			LastName = "Amamiya",
			Age = 16,
			Email = "joker@mail.com",
			PhoneNumber = "0983471892"
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
	public async Task Patch_Should_ReturnNoContentResult_WhenOwnerExists()
	{
		// Arrange
		var delta = new Delta<OwnerUpdateDto>();
		delta.TrySetPropertyValue(nameof(OwnerUpdateDto.FirstName), "Goro");
		delta.TrySetPropertyValue(nameof(OwnerUpdateDto.Email), "crow@mail.com");

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
	public async Task Delete_Should_ReturnNoContentResult_WhenOwnerExists()
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
