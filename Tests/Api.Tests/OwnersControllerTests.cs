using Api.Tests.Fixtures;
using Application.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
		_fixture.OwnersService
			.Setup(s => s.GetAll())
			.Returns(_fixture.OwnerDtoQuery);

		// Act
		var result = _fixture.OwnersController.Get();
		var objectResult = result.Result as OkObjectResult;
		var queryResult = objectResult?.Value as IQueryable<OwnerDto>;

		// Assert
		Assert.IsInstanceOfType<ActionResult<IQueryable<OwnerDto>>>(result);
		Assert.AreEqual(StatusCodes.Status200OK, objectResult?.StatusCode);
		Assert.IsNotNull(queryResult);
		Assert.AreEqual(_fixture.OwnerDtosCount, queryResult.Count());
	}

	[TestMethod]
	public void Get_Should_ReturnSingleResultOfOwnerDto_WhenOwnerExists()
	{
		// Arrange
		_fixture.OwnersService
			.Setup(s => s.GetById(It.IsAny<Guid>()))
			.Returns(_fixture.OwnerDtoQuery);

		// Act
		var result = _fixture.OwnersController.Get(_fixture.Id);
		var objectResult = result.Result as OkObjectResult;
		var singleResult = objectResult?.Value as SingleResult<OwnerDto>;

		// Assert
		Assert.IsNotNull(result);
		Assert.IsInstanceOfType<ActionResult<SingleResult<OwnerDto>>>(result);
		Assert.AreEqual(StatusCodes.Status200OK, objectResult?.StatusCode);
		Assert.IsNotNull(singleResult);
	}

	[TestMethod]
	public async Task Post_Should_ReturnActionResultOfOwnerDto_WhenOwnerDtoIsValid()
	{
		// Arrange
		_fixture.OwnersService
			.Setup(s => s.CreateAsync(It.IsAny<OwnerDto>()))
			.ReturnsAsync(_fixture.OwnerDto);

		// Act
		var result = await _fixture.OwnersController.Post(_fixture.OwnerDto);
		var objectResult = result.Result as CreatedAtActionResult;
		var ownerDto = objectResult?.Value as OwnerDto;

		// Assert
		Assert.IsNotNull(result);
		Assert.IsInstanceOfType<ActionResult<OwnerDto>>(result);
		Assert.AreEqual(StatusCodes.Status201Created, objectResult?.StatusCode);
		Assert.IsNotNull(ownerDto);
	}

	[TestMethod]
	public async Task Put_Should_ReturnNoContentResult_WhenOwnerExists()
	{
		// Act
		var result = await _fixture.OwnersController.Put(_fixture.Id, _fixture.OwnerDto);
		var objectResult = result as NoContentResult;

		// Assert
		Assert.IsNotNull(result);
		Assert.IsInstanceOfType<NoContentResult>(result);
		Assert.AreEqual(StatusCodes.Status204NoContent, objectResult?.StatusCode);
	}

	[TestMethod]
	public async Task Patch_Should_ReturnNoContentResult_WhenOwnerExists()
	{
		// Act
		var result = await _fixture.OwnersController.Patch(_fixture.Id, _fixture.OwnerDtoDelta);
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
		var result = await _fixture.OwnersController.Delete(_fixture.Id);
		var objectResult = result as NoContentResult;

		// Assert
		Assert.IsNotNull(result);
		Assert.IsInstanceOfType<NoContentResult>(result);
		Assert.AreEqual(StatusCodes.Status204NoContent, objectResult?.StatusCode);
	}
}
