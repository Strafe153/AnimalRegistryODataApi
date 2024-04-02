using Api.Tests.Fixtures;
using Application.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
		// Arrange
		_fixture.AnimalsService
			.Setup(s => s.GetAll())
			.Returns(_fixture.AnimalDtoQuery);

		// Act
		var result = _fixture.AnimalsController.Get();
		var objectResult = result.Result as OkObjectResult;
		var queryResult = objectResult?.Value as IQueryable<AnimalDto>;

		// Assert
		Assert.IsInstanceOfType<ActionResult<IQueryable<AnimalDto>>>(result);
		Assert.AreEqual(StatusCodes.Status200OK, objectResult?.StatusCode);
		Assert.IsNotNull(queryResult);
		Assert.AreEqual(_fixture.AnimalDtosCount, queryResult.Count());
	}

	[TestMethod]
	public void Get_Should_ReturnSingleResultOfAnimalDto_WhenAnimalExists()
	{
		// Arrange
		_fixture.AnimalsService
			.Setup(s => s.GetById(It.IsAny<Guid>()))
			.Returns(_fixture.AnimalDtoQuery);

		// Act
		var result = _fixture.AnimalsController.Get(_fixture.Id);
		var objectResult = result.Result as OkObjectResult;
		var singleResult = objectResult?.Value as SingleResult<AnimalDto>;

		// Assert
		Assert.IsNotNull(result);
		Assert.IsInstanceOfType<ActionResult<SingleResult<AnimalDto>>>(result);
		Assert.AreEqual(StatusCodes.Status200OK, objectResult?.StatusCode);
		Assert.IsNotNull(singleResult);
	}

	[TestMethod]
	public async Task Post_Should_ReturnActionResultOfAnimalDto_WhenAnimalDtoIsValid()
	{
		// Arrange
		_fixture.AnimalsService
			.Setup(s => s.CreateAsync(It.IsAny<AnimalDto>()))
			.ReturnsAsync(_fixture.AnimalDto);

		// Act
		var result = await _fixture.AnimalsController.Post(_fixture.AnimalDto);
		var objectResult = result.Result as CreatedAtActionResult;
		var animalDto = objectResult?.Value as AnimalDto;

		// Assert
		Assert.IsNotNull(result);
		Assert.IsInstanceOfType<ActionResult<AnimalDto>>(result);
		Assert.AreEqual(StatusCodes.Status201Created, objectResult?.StatusCode);
		Assert.IsNotNull(animalDto);
	}

	[TestMethod]
	public async Task Put_Should_ReturnNoContentResult_WhenAnimalExists()
	{
		// Act
		var result = await _fixture.AnimalsController.Put(_fixture.Id, _fixture.AnimalDto);
		var objectResult = result as NoContentResult;

		// Assert
		Assert.IsNotNull(result);
		Assert.IsInstanceOfType<NoContentResult>(result);
		Assert.AreEqual(StatusCodes.Status204NoContent, objectResult?.StatusCode);
	}

	[TestMethod]
	public async Task Patch_Should_ReturnNoContentResult_WhenAnimalExists()
	{
		// Act
		var result = await _fixture.AnimalsController.Patch(_fixture.Id, _fixture.AnimalDtoDelta);
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
		var result = await _fixture.AnimalsController.Delete(_fixture.Id);
		var objectResult = result as NoContentResult;

		// Assert
		Assert.IsNotNull(result);
		Assert.IsInstanceOfType<NoContentResult>(result);
		Assert.AreEqual(StatusCodes.Status204NoContent, objectResult?.StatusCode);
	}
}
