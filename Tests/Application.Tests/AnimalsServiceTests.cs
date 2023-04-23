using Application.Tests.Fixtures;
using Core.DTOs;
using Core.Entities;
using Core.Exceptions;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Application.Tests;

[TestClass]
public class AnimalsServiceTests
{
    private readonly AnimalsServiceFixture _fixture;

    public AnimalsServiceTests()
    {
        _fixture = new AnimalsServiceFixture();
    }

    [TestMethod]
    public void GetAll_Should_ReturnIQueryableOfAnimalDto()
    {
        // Arrange
        _fixture.Context
            .Setup(c => c.Animals)
            .Returns(_fixture.AnimalsDbSet);

        _fixture.Mapper
            .Setup(m => m.ProjectTo<AnimalDto>(It.IsAny<IQueryable<Animal>>(), It.IsAny<object>()))
            .Returns(_fixture.AnimalDtosQuery);

        // Act
        var result = _fixture.AnimalsService.GetAll();

        // Assert
        result.AsEnumerable().Should().NotBeEmpty();
    }

    [TestMethod]
    public void GetById_Should_ReturnIQueryableOfAnimalDto()
    {
        // Arrange
        _fixture.Context
            .Setup(c => c.Animals)
            .Returns(_fixture.AnimalsDbSet);

        _fixture.Mapper
            .Setup(m => m.ProjectTo<AnimalDto>(It.IsAny<IQueryable<Animal>>(), It.IsAny<object>()))
            .Returns(_fixture.AnimalDtosQuery);

        // Act
        var result = _fixture.AnimalsService.GetById(_fixture.Id);

        // Assert
        result.Count().Should().Be(2);
    }

    [TestMethod]
    public async Task CreateAsync_Should_ReturnAnimalDto_WhenAnimalDtoIsValid()
    {
        // Arrange
        _fixture.Context
            .Setup(c => c.Animals)
            .Returns(_fixture.AnimalsDbSet);

        _fixture.Context
            .Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _fixture.AnimalsService.CreateAsync(_fixture.AnimalDto);

        // Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public async Task CreateAsync_Should_ThrowOperationFailedException_WhenAnimalDtoIsInvalid()
    {
        // Arrange
        _fixture.Context
            .Setup(c => c.Animals.Add(It.IsAny<Animal>()))
            .Throws(new OperationFailedException(_fixture.Id.ToString()));

        // Act
        var result = async () => await _fixture.AnimalsService.CreateAsync(_fixture.AnimalDto);

        // Assert
        await result.Should().ThrowAsync<OperationFailedException>();
    }

    [TestMethod]
    public async Task UpdateAsync_Should_ReturnTask_WhenAnimalDtoIsValid()
    {
        // Act
        var result = async () => await _fixture.AnimalsService.UpdateAsync(_fixture.Id, _fixture.AnimalDto);

        // Assert
        await result.Should().NotThrowAsync<NullReferenceException>();
        await result.Should().NotThrowAsync<OperationFailedException>();
    }

    [TestMethod]
    public async Task UpdateAsync_Should_ThrowNullReferenceException_WhenAnimalDoesNotExist()
    {
        // Arrange
        _fixture.Context
            .Setup(c => c.Animals)
            .Returns(_fixture.AnimalsDbSet);

        _fixture.Context
            .Setup(c => c.Animals.FindAsync(It.IsAny<object[]>()))
            .ReturnsAsync((Animal)null!);

        // Act
        var result = async () => await _fixture.AnimalsService.UpdateAsync(_fixture.Id, _fixture.AnimalDto);

        // Assert
        await result.Should().ThrowAsync<NullReferenceException>();
    }

    [TestMethod]
    public async Task UpdateAsync_Should_ThrowOperationFailedException_WhenAnimalDtoIsInvalid()
    {
        // Arrange
        _fixture.Context
            .Setup(c => c.Animals)
            .Returns(_fixture.AnimalsDbSet);

        _fixture.Context
            .Setup(c => c.Animals.FindAsync(It.IsAny<object[]>()))
            .ReturnsAsync(_fixture.Animal);

        // Act
        var result = async () => await _fixture.AnimalsService.UpdateAsync(_fixture.Id, _fixture.AnimalDto);

        // Assert
        await result.Should().ThrowAsync<OperationFailedException>();
    }

    [TestMethod]
    public async Task DeleteAsync_Should_ReturnTask_WhenAnimalDtoIsValid()
    {
        // Act
        var result = async () => await _fixture.AnimalsService.DeleteAsync(_fixture.Id);

        // Assert
        await result.Should().NotThrowAsync<NullReferenceException>();
        await result.Should().NotThrowAsync<OperationFailedException>();
    }

    [TestMethod]
    public async Task DeleteAsync_Should_ThrowOperationFailedException_WhenOperationFails()
    {
        // Arrange
        _fixture.Context
            .Setup(c => c.Animals)
            .Returns(_fixture.AnimalsDbSet);

        _fixture.Context
            .Setup(c => c.Animals.FindAsync(It.IsAny<object[]>()))
            .ReturnsAsync(_fixture.Animal);

        // Act
        var result = async () => await _fixture.AnimalsService.DeleteAsync(_fixture.Id);

        // Assert
        await result.Should().ThrowAsync<OperationFailedException>();
    }

    [TestMethod]
    public async Task DeleteAsync_Should_ThrowNullReferenceException_WhenAnimalDoesNotExist()
    {
        // Arrange
        _fixture.Context
            .Setup(c => c.Animals)
            .Returns(_fixture.AnimalsDbSet);

        _fixture.Context
            .Setup(c => c.Animals.FindAsync(It.IsAny<object[]>()))
            .ReturnsAsync((Animal)null!);

        // Act
        var result = async () => await _fixture.AnimalsService.DeleteAsync(_fixture.Id);

        // Assert
        await result.Should().ThrowAsync<NullReferenceException>();
    }
}
