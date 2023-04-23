using Application.Tests.Fixtures;
using Core.DTOs;
using Core.Entities;
using Core.Exceptions;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Application.Tests;

[TestClass]
public class OwnersServiceTests
{
    private readonly OwnersServiceFixture _fixture;

    public OwnersServiceTests()
    {
        _fixture = new OwnersServiceFixture();
    }

    [TestMethod]
    public void GetAll_Should_ReturnIQueryableOfOwnerDto()
    {
        // Arrange
        _fixture.Context
            .Setup(c => c.Owners)
            .Returns(_fixture.OwnersDbSet);

        _fixture.Mapper
            .Setup(m => m.ProjectTo<OwnerDto>(It.IsAny<IQueryable<Owner>>(), It.IsAny<object>()))
            .Returns(_fixture.OwnerDtosQuery);

        // Act
        var result = _fixture.OwnersService.GetAll();

        // Assert
        result.AsEnumerable().Should().NotBeEmpty();
    }

    [TestMethod]
    public void GetById_Should_ReturnIQueryableOfOwnerDto()
    {
        // Arrange
        _fixture.Context
            .Setup(c => c.Owners)
            .Returns(_fixture.OwnersDbSet);

        _fixture.Mapper
            .Setup(m => m.ProjectTo<OwnerDto>(It.IsAny<IQueryable<Owner>>(), It.IsAny<object>()))
            .Returns(_fixture.OwnerDtosQuery);

        // Act
        var result = _fixture.OwnersService.GetById(_fixture.Id);

        // Assert
        result.Count().Should().Be(2);
    }

    [TestMethod]
    public async Task CreateAsync_Should_ReturnOwnerDto_WhenOwnerDtoIsValid()
    {
        // Arrange
        _fixture.Context
            .Setup(c => c.Owners)
            .Returns(_fixture.OwnersDbSet);

        _fixture.Context
            .Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _fixture.OwnersService.CreateAsync(_fixture.OwnerDto);

        // Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public async Task CreateAsync_Should_ThrowOperationFailedException_WhenOwnerDtoIsInvalid()
    {
        // Arrange
        _fixture.Context
            .Setup(c => c.Owners.Add(It.IsAny<Owner>()))
            .Throws(new OperationFailedException(_fixture.Id.ToString()));

        // Act
        var result = async () => await _fixture.OwnersService.CreateAsync(_fixture.OwnerDto);

        // Assert
        await result.Should().ThrowAsync<OperationFailedException>();
    }

    [TestMethod]
    public async Task UpdateAsync_Should_ReturnTask_WhenOwnerDtoIsValid()
    {
        // Act
        var result = async () => await _fixture.OwnersService.UpdateAsync(_fixture.Id, _fixture.OwnerDto);

        // Assert
        await result.Should().NotThrowAsync<NullReferenceException>();
        await result.Should().NotThrowAsync<OperationFailedException>();
    }

    [TestMethod]
    public async Task UpdateAsync_Should_ThrowNullReferenceException_WhenOwnerDoesNotExist()
    {
        // Arrange
        _fixture.Context
            .Setup(c => c.Owners)
            .Returns(_fixture.OwnersDbSet);

        _fixture.Context
            .Setup(c => c.Owners.FindAsync(It.IsAny<object[]>()))
            .ReturnsAsync((Owner)null!);

        // Act
        var result = async () => await _fixture.OwnersService.UpdateAsync(_fixture.Id, _fixture.OwnerDto);

        // Assert
        await result.Should().ThrowAsync<NullReferenceException>();
    }

    [TestMethod]
    public async Task UpdateAsync_Should_ThrowOperationFailedException_WhenOwnerDtoIsInvalid()
    {
        // Arrange
        _fixture.Context
            .Setup(c => c.Owners)
            .Returns(_fixture.OwnersDbSet);

        _fixture.Context
            .Setup(c => c.Owners.FindAsync(It.IsAny<object[]>()))
            .ReturnsAsync(_fixture.Owner);

        // Act
        var result = async () => await _fixture.OwnersService.UpdateAsync(_fixture.Id, _fixture.OwnerDto);

        // Assert
        await result.Should().ThrowAsync<OperationFailedException>();
    }

    [TestMethod]
    public async Task DeleteAsync_Should_ReturnTask_WhenOwnerDtoIsValid()
    {
        // Act
        var result = async () => await _fixture.OwnersService.DeleteAsync(_fixture.Id);

        // Assert
        await result.Should().NotThrowAsync<NullReferenceException>();
        await result.Should().NotThrowAsync<OperationFailedException>();
    }

    [TestMethod]
    public async Task DeleteAsync_Should_ThrowOperationFailedException_WhenOperationFails()
    {
        // Arrange
        _fixture.Context
            .Setup(c => c.Owners)
            .Returns(_fixture.OwnersDbSet);

        _fixture.Context
            .Setup(c => c.Owners.FindAsync(It.IsAny<object[]>()))
            .ReturnsAsync(_fixture.Owner);

        // Act
        var result = async () => await _fixture.OwnersService.DeleteAsync(_fixture.Id);

        // Assert
        await result.Should().ThrowAsync<OperationFailedException>();
    }

    [TestMethod]
    public async Task DeleteAsync_Should_ThrowNullReferenceException_WhenOwnerDoesNotExist()
    {
        // Arrange
        _fixture.Context
            .Setup(c => c.Owners)
            .Returns(_fixture.OwnersDbSet);

        _fixture.Context
            .Setup(c => c.Owners.FindAsync(It.IsAny<object[]>()))
            .ReturnsAsync((Owner)null!);

        // Act
        var result = async () => await _fixture.OwnersService.DeleteAsync(_fixture.Id);

        // Assert
        await result.Should().ThrowAsync<NullReferenceException>();
    }
}
