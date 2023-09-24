using Application.Tests.Fixtures;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

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
        _fixture.Session
            .Setup(s => s.GetAll())
            .Returns(_fixture.GetAllAnimalsQuery);

        // Act
        
        var result = _fixture.AnimalsService.GetAll();

        // Assert
        result.Count().Should().Be(_fixture.AnimalsCount);
    }

    [TestMethod]
    public void GetById_Should_ReturnIQueryableOfAnimalDto()
    {
        // Arrange
        _fixture.Session
            .Setup(s => s.GetById(It.IsAny<Guid>()))
            .Returns(_fixture.GetByIdAnimalsQuery);

        // Act
        var result = _fixture.AnimalsService.GetById(_fixture.Id);

        // Assert
        result.Count().Should().Be(1);
    }

    [TestMethod]
    public async Task CreateAsync_Should_ReturnAnimalDto_WhenAnimalDtoIsValid()
    {
        // Act
        var result = await _fixture.AnimalsService.CreateAsync(_fixture.AnimalDto);

        // Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public async Task CreateAsync_Should_ThrowOperationFailedException_WhenAnimalDtoIsInvalid()
    {
        // Arrange
        _fixture.Session
            .Setup(c => c.GetById(It.IsAny<Guid>()))
            .Returns(_fixture.GetByIdAnimalsQuery);

        _fixture.TransactionRunner
            .Setup(r => r.RunInTransactionAsync(
                It.IsAny<Func<Task>>(),
                It.IsAny<IMapperSession<Animal>>(),
                It.IsAny<string>()))
            .Throws(new OperationFailedException(_fixture.Id.ToString()));

        // Act
        var result = async () => await _fixture.AnimalsService.CreateAsync(_fixture.AnimalDto);

        // Assert
        await result.Should().ThrowAsync<OperationFailedException>();
    }

    [TestMethod]
    public async Task UpdateAsync_Should_ReturnTask_WhenAnimalDtoIsValid()
    {
        // Arrange
        _fixture.Session
            .Setup(c => c.GetById(It.IsAny<Guid>()))
            .Returns(_fixture.GetByIdAnimalsQuery);

        // Act
        var result = async () => await _fixture.AnimalsService.UpdateAsync(_fixture.Id, _fixture.AnimalDto);

        // Assert
        await result.Should().NotThrowAsync<NullReferenceException>();
        await result.Should().NotThrowAsync<OperationFailedException>();
    }

    [TestMethod]
    public async Task UpdateAsync_Should_ReturnTask_WhenAnimalDeltaIsValid()
    {
        // Arrange
        _fixture.Session
            .Setup(c => c.GetById(It.IsAny<Guid>()))
            .Returns(_fixture.GetByIdAnimalsQuery);

        // Act
        var result = async () => await _fixture.AnimalsService.UpdateAsync(_fixture.Id, _fixture.AnimalDtoDelta);

        // Assert
        await result.Should().NotThrowAsync<NullReferenceException>();
        await result.Should().NotThrowAsync<OperationFailedException>();
    }

    [TestMethod]
    public async Task UpdateAsync_Should_ThrowNullReferenceException_WhenAnimalDoesNotExist()
    {
        // Arrange
        _fixture.Session
            .Setup(s => s.GetById(It.IsAny<Guid>()))
            .Returns(_fixture.GetByIdEmptyAnimalsQuery);

        // Act
        var result = async () => await _fixture.AnimalsService.UpdateAsync(_fixture.Id, _fixture.AnimalDto);

        // Assert
        await result.Should().ThrowAsync<NullReferenceException>();
    }

    [TestMethod]
    public async Task UpdateAsync_Should_ThrowNullReferenceException_WithDeltaWhenAnimalDoesNotExist()
    {
        // Arrange
        _fixture.Session
            .Setup(s => s.GetById(It.IsAny<Guid>()))
            .Returns(_fixture.GetByIdEmptyAnimalsQuery);

        // Act
        var result = async () => await _fixture.AnimalsService.UpdateAsync(_fixture.Id, _fixture.AnimalDtoDelta);

        // Assert
        await result.Should().ThrowAsync<NullReferenceException>();
    }

    [TestMethod]
    public async Task UpdateAsync_Should_ThrowOperationFailedException_WhenAnimalDtoIsInvalid()
    {
        // Arrange
        _fixture.Session
            .Setup(c => c.GetById(It.IsAny<Guid>()))
            .Returns(_fixture.GetByIdAnimalsQuery);

        _fixture.TransactionRunner
            .Setup(r => r.RunInTransactionAsync(
                It.IsAny<Func<Task>>(),
                It.IsAny<IMapperSession<Animal>>(),
                It.IsAny<string>()))
            .Throws(new OperationFailedException(_fixture.Id.ToString()));

        // Act
        var result = async () => await _fixture.AnimalsService.UpdateAsync(_fixture.Id, _fixture.AnimalDto);

        // Assert
        await result.Should().ThrowAsync<OperationFailedException>();
    }

    [TestMethod]
    public async Task UpdateAsync_Should_ThrowOperationFailedException_WhenAnimalDeltaIsInvalid()
    {
        // Arrange
        _fixture.Session
            .Setup(c => c.GetById(It.IsAny<Guid>()))
            .Returns(_fixture.GetByIdAnimalsQuery);

        _fixture.TransactionRunner
            .Setup(r => r.RunInTransactionAsync(
                It.IsAny<Func<Task>>(),
                It.IsAny<IMapperSession<Animal>>(),
                It.IsAny<string>()))
            .Throws(new OperationFailedException(_fixture.Id.ToString()));

        // Act
        var result = async () => await _fixture.AnimalsService.UpdateAsync(_fixture.Id, _fixture.AnimalDtoDelta);

        // Assert
        await result.Should().ThrowAsync<OperationFailedException>();
    }

    [TestMethod]
    public async Task DeleteAsync_Should_ReturnTask_WhenAnimalDtoIsValid()
    {
        // Arrange
        _fixture.Session
            .Setup(c => c.GetById(It.IsAny<Guid>()))
            .Returns(_fixture.GetByIdAnimalsQuery);

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
        _fixture.Session
            .Setup(s => s.GetById(It.IsAny<Guid>()))
            .Returns(_fixture.GetByIdAnimalsQuery);

        _fixture.TransactionRunner
            .Setup(r => r.RunInTransactionAsync(
                It.IsAny<Func<Task>>(),
                It.IsAny<IMapperSession<Animal>>(),
                It.IsAny<string>()))
            .Throws(new OperationFailedException(_fixture.Id.ToString()));

        // Act
        var result = async () => await _fixture.AnimalsService.DeleteAsync(_fixture.Id);

        // Assert
        await result.Should().ThrowAsync<OperationFailedException>();
    }

    [TestMethod]
    public async Task DeleteAsync_Should_ThrowNullReferenceException_WhenAnimalDoesNotExist()
    {
        // Arrange
        _fixture.Session
            .Setup(s => s.GetById(It.IsAny<Guid>()))
            .Returns(_fixture.GetByIdEmptyAnimalsQuery);

        // Act
        var result = async () => await _fixture.AnimalsService.DeleteAsync(_fixture.Id);

        // Assert
        await result.Should().ThrowAsync<NullReferenceException>();
    }
}
