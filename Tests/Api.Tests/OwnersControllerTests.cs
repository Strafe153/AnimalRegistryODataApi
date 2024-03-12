using Api.Tests.Fixtures;
using Domain.DTOs;
using FluentAssertions;
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
        var objectResult = result.Result.As<OkObjectResult>();
        var queryResult = objectResult.Value.As<IQueryable<OwnerDto>>();

        // Assert
        result.Should().BeOfType<ActionResult<IQueryable<OwnerDto>>>();
        objectResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        queryResult.Should().NotBeNull();
        queryResult.Count().Should().Be(_fixture.OwnerDtosCount);
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
        var objectResult = result.Result.As<OkObjectResult>();
        var singleResult = objectResult.Value.As<SingleResult<OwnerDto>>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<ActionResult<SingleResult<OwnerDto>>>();
        objectResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        singleResult.Should().NotBeNull();
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
        var objectResult = result.Result.As<CreatedAtActionResult>();
        var ownerDto = objectResult.Value.As<OwnerDto>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<ActionResult<OwnerDto>>();
        objectResult.StatusCode.Should().Be(StatusCodes.Status201Created);
        ownerDto.Should().NotBeNull();
    }

    [TestMethod]
    public async Task Put_Should_ReturnNoContentResult_WhenOwnerExists()
    {
        // Act
        var result = await _fixture.OwnersController.Put(_fixture.Id, _fixture.OwnerDto);
        var objectResult = result.As<NoContentResult>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<NoContentResult>();
        objectResult.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }

    [TestMethod]
    public async Task Patch_Should_ReturnNoContentResult_WhenOwnerExists()
    {
        // Act
        var result = await _fixture.OwnersController.Patch(_fixture.Id, _fixture.OwnerDtoDelta);
        var objectResult = result.As<NoContentResult>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<NoContentResult>();
        objectResult.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }

    [TestMethod]
    public async Task Delete_Should_ReturnNoContentResult_WhenOwnerExists()
    {
        // Act
        var result = await _fixture.OwnersController.Delete(_fixture.Id);
        var objectResult = result.As<NoContentResult>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<NoContentResult>();
        objectResult.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }
}
