using Application.Tests.Fixtures;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

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
		_fixture.Session
			.Setup(s => s.GetAll())
			.Returns(_fixture.GetAllOwnersQuery);

		// Act
		var result = _fixture.OwnersService.GetAll();

		// Assert
		Assert.AreEqual(_fixture.OwnersCount, result.Count());
	}

	[TestMethod]
	public void GetById_Should_ReturnIQueryableOfOwnerDto()
	{
		// Arrange
		_fixture.Session
			.Setup(s => s.GetById(It.IsAny<Guid>()))
			.Returns(_fixture.GetByIdOwnersQuery);

		// Act
		var result = _fixture.OwnersService.GetById(_fixture.Id);

		// Assert
		Assert.AreEqual(1, result.Count());
	}

	[TestMethod]
	public async Task CreateAsync_Should_ReturnOwnerDto_WhenOwnerDtoIsValid()
	{
		// Act
		var result = await _fixture.OwnersService.CreateAsync(_fixture.OwnerDto);

		// Assert
		Assert.IsNotNull(result);
	}

	[TestMethod]
	public async Task CreateAsync_Should_ThrowOperationFailedException_WhenOwnerDtoIsInvalid()
	{
		// Arrange
		_fixture.Session
			.Setup(c => c.GetById(It.IsAny<Guid>()))
			.Returns(_fixture.GetByIdOwnersQuery);

		_fixture.TransactionRunner
			.Setup(r => r.RunInTransactionAsync(
				It.IsAny<Func<Task>>(),
				It.IsAny<IMapperSession<Owner>>(),
				It.IsAny<string>()))
			.Throws(new OperationFailedException(_fixture.Id.ToString()));

		// Act
		Task result() => _fixture.OwnersService.CreateAsync(_fixture.OwnerDto);

		// Assert
		await Assert.ThrowsExceptionAsync<OperationFailedException>(result);
	}

	[TestMethod]
	public async Task UpdateAsync_Should_ReturnTask_WhenOwnerDtoIsValid()
	{
		// Arrange
		_fixture.Session
			.Setup(s => s.GetById(It.IsAny<Guid>()))
			.Returns(_fixture.GetByIdOwnersQuery);

		try
		{
			// Act
			await _fixture.OwnersService.UpdateAsync(_fixture.Id, _fixture.OwnerDto);
		}
		catch
		{
			// Assert
			Assert.Fail();
		}
	}

	[TestMethod]
	public async Task UpdateAsync_Should_ReturnTask_WhenOwnerDeltaIsValid()
	{
		// Arrange
		_fixture.Session
			.Setup(s => s.GetById(It.IsAny<Guid>()))
			.Returns(_fixture.GetByIdOwnersQuery);

		try
		{
			// Act
			await _fixture.OwnersService.UpdateAsync(_fixture.Id, _fixture.OwnerDtoDelta);
		}
		catch
		{
			// Assert
			Assert.Fail();
		}
	}

	[TestMethod]
	public async Task UpdateAsync_Should_ThrowNullReferenceException_WhenOwnerDoesNotExist()
	{
		// Arrange
		_fixture.Session
			.Setup(s => s.GetById(It.IsAny<Guid>()))
			.Returns(_fixture.GetByIdEmptyOwnersQuery);

		// Act
		Task result() => _fixture.OwnersService.UpdateAsync(_fixture.Id, _fixture.OwnerDto);

		// Assert
		await Assert.ThrowsExceptionAsync<NullReferenceException>(result);
	}

	[TestMethod]
	public async Task UpdateAsync_Should_ThrowNullReferenceException_WithDeltaWhenOwnerDoesNotExist()
	{
		// Arrange
		_fixture.Session
			.Setup(s => s.GetById(It.IsAny<Guid>()))
			.Returns(_fixture.GetByIdEmptyOwnersQuery);

		// Act
		Task result() => _fixture.OwnersService.UpdateAsync(_fixture.Id, _fixture.OwnerDtoDelta);

		// Assert
		await Assert.ThrowsExceptionAsync<NullReferenceException>(result);
	}

	[TestMethod]
	public async Task UpdateAsync_Should_ThrowOperationFailedException_WhenOwnerDtoIsInvalid()
	{
		// Arrange
		_fixture.Session
			.Setup(s => s.GetById(It.IsAny<Guid>()))
			.Returns(_fixture.GetByIdOwnersQuery);

		_fixture.TransactionRunner
			.Setup(r => r.RunInTransactionAsync(
				It.IsAny<Func<Task>>(),
				It.IsAny<IMapperSession<Owner>>(),
				It.IsAny<string>()))
			.Throws(new OperationFailedException(_fixture.Id.ToString()));

		// Act
		Task result() => _fixture.OwnersService.UpdateAsync(_fixture.Id, _fixture.OwnerDto);

		// Assert
		await Assert.ThrowsExceptionAsync<OperationFailedException>(result);
	}

	[TestMethod]
	public async Task UpdateAsync_Should_ThrowOperationFailedException_WhenOwnerDeltaIsInvalid()
	{
		// Arrange
		_fixture.Session
			.Setup(s => s.GetById(It.IsAny<Guid>()))
			.Returns(_fixture.GetByIdOwnersQuery);

		_fixture.TransactionRunner
			.Setup(r => r.RunInTransactionAsync(
				It.IsAny<Func<Task>>(),
				It.IsAny<IMapperSession<Owner>>(),
				It.IsAny<string>()))
			.Throws(new OperationFailedException(_fixture.Id.ToString()));

		// Act
		Task result() => _fixture.OwnersService.UpdateAsync(_fixture.Id, _fixture.OwnerDtoDelta);

		// Assert
		await Assert.ThrowsExceptionAsync<OperationFailedException>(result);
	}

	[TestMethod]
	public async Task DeleteAsync_Should_ReturnTask_WhenOwnerDtoIsValid()
	{
		// Arrange
		_fixture.Session
			.Setup(s => s.GetById(It.IsAny<Guid>()))
			.Returns(_fixture.GetByIdOwnersQuery);

		try
		{
			// Act
			await _fixture.OwnersService.DeleteAsync(_fixture.Id);
		}
		catch
		{
			// Assert
			Assert.Fail();
		}
	}

	[TestMethod]
	public async Task DeleteAsync_Should_ThrowOperationFailedException_WhenOperationFails()
	{
		// Arrange
		_fixture.Session
			.Setup(s => s.GetById(It.IsAny<Guid>()))
			.Returns(_fixture.GetByIdOwnersQuery);

		_fixture.TransactionRunner
			.Setup(r => r.RunInTransactionAsync(
				It.IsAny<Func<Task>>(),
				It.IsAny<IMapperSession<Owner>>(),
				It.IsAny<string>()))
			.Throws(new OperationFailedException(_fixture.Id.ToString()));

		// Act
		Task result() => _fixture.OwnersService.DeleteAsync(_fixture.Id);

		// Assert
		await Assert.ThrowsExceptionAsync<OperationFailedException>(result);
	}

	[TestMethod]
	public async Task DeleteAsync_Should_ThrowNullReferenceException_WhenOwnerDoesNotExist()
	{
		// Arrange
		_fixture.Session
			.Setup(s => s.GetById(It.IsAny<Guid>()))
			.Returns(_fixture.GetByIdEmptyOwnersQuery);

		// Act
		Task result() => _fixture.OwnersService.DeleteAsync(_fixture.Id);

		// Assert
		await Assert.ThrowsExceptionAsync<NullReferenceException>(result);
	}
}
