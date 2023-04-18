using Application.Tests.Fixtures;
using Core.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Application.Tests;

[TestClass]
public class AnimalsControllerTests
{
    private readonly AnimalsControllerFixture _fixture;

    public AnimalsControllerTests()
    {
        _fixture = new AnimalsControllerFixture();
    }

    [TestMethod]
    public void Get_Should_ReturnIQueryableOfAnimalDto()
    {
        // Arrange
        _fixture.AnimalsService
            .Setup(s => s.GetAllAsync())
            .Returns(_fixture.AnimalDtoQuery);

        // Act
        var result = _fixture.AnimalsController.Get();
        var objectResult = result.Result.As<OkObjectResult>();
        var queryResult = objectResult.Value.As<IQueryable<AnimalDto>>();

        // Assert
        result.Should().BeOfType<ActionResult<IQueryable<AnimalDto>>>();
        objectResult.StatusCode.Should().Be(200);
        queryResult.Should().NotBeNull();
    }

    [TestMethod]
    public void Get_Should_ReturnSingleResultOfAnimalDto_WhenAnimalExists()
    {
        // Arrange
        _fixture.AnimalsService
            .Setup(s => s.GetByIdAsync(It.IsAny<Guid>()))
            .Returns(_fixture.AnimalDtoQuery);

        // Act
        var result = _fixture.AnimalsController.Get(_fixture.Id);
        var objectResult = result.Result.As<OkObjectResult>();
        var singleResult = objectResult.Value.As<SingleResult<AnimalDto>>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<ActionResult<SingleResult<AnimalDto>>>();
        objectResult.StatusCode.Should().Be(200);
        singleResult.Should().NotBeNull();
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
        var objectResult = result.Result.As<CreatedAtActionResult>();
        var ownerDto = objectResult.Value.As<AnimalDto>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<ActionResult<AnimalDto>>();
        objectResult.StatusCode.Should().Be(201);
        ownerDto.Should().NotBeNull();
    }

    [TestMethod]
    public async Task Put_Should_ReturnNoContentResult_WhenAnimalExists()
    {
        // Act
        var result = await _fixture.AnimalsController.Put(_fixture.Id, _fixture.AnimalDto);
        var objectResult = result.As<NoContentResult>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<NoContentResult>();
        objectResult.StatusCode.Should().Be(204);
    }

    [TestMethod]
    public async Task Delete_Should_ReturnNoContentResult_WhenAnimalExists()
    {
        // Act
        var result = await _fixture.AnimalsController.Delete(_fixture.Id);
        var objectResult = result.As<NoContentResult>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<NoContentResult>();
        objectResult.StatusCode.Should().Be(204);
    }
}
