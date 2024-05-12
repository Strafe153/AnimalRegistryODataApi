using Application.DTOs;
using Application.Services.Interfaces;
using Domain.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.RateLimiting;
using System.Net.Mime;
using ProblemDetails = Domain.Shared.ProblemDetails;

namespace AnimalRegistryODataApi.Controllers;

[EnableRateLimiting(RateLimitingConstants.TokenBucket)]
public class OwnersController : ODataController
{
	private readonly IOwnersService _ownersService;

	public OwnersController(IOwnersService ownersService)
	{
		_ownersService = ownersService;
	}

	/// <summary>
	/// Fetches a collection of owners
	/// </summary>
	/// <returns>A collection of owners</returns>
	/// <response code="200">Returns a collection of owners</response>
	[EnableQuery(PageSize = 10)]
	[ResponseCache(CacheProfileName = CacheConstants.Default)]
	[Consumes(MediaTypeNames.Application.Json)]
	[Produces(MediaTypeNames.Application.Json)]
	[ProducesResponseType(typeof(IQueryable<OwnerDto>), StatusCodes.Status200OK)]
	public ActionResult<IQueryable<OwnerDto>> Get() => Ok(_ownersService.GetAll());

	/// <summary>
	/// Fetches an owner by the specified key
	/// </summary>
	/// <param name="key">An owner's identifier</param>
	/// <returns>An animal by the specified key</returns>
	/// <response code="200">Returns an owner by the specified identifier</response>
	[EnableQuery]
	[ResponseCache(CacheProfileName = CacheConstants.Default)]
	[Consumes(MediaTypeNames.Application.Json)]
	[Produces(MediaTypeNames.Application.Json)]
	[ProducesResponseType(typeof(SingleResult<OwnerDto>), StatusCodes.Status200OK)]
	public ActionResult<SingleResult<OwnerDto>> Get([FromODataUri] Guid key) =>
		Ok(SingleResult.Create(_ownersService.GetById(key)));

	/// <summary>
	/// Creates an owner
	/// </summary>
	/// <param name="createDto">An owner creation DTO</param>
	/// <returns>The newly created owner</returns>
	/// <remarks>
	/// Sample request:
	///
	///     POST /odata/v1/owners
	///     {
	///         "firstName": "Goro",
	///         "lastName": "Akechi",
	///         "age": 17,
	///         "email": "crow@gmail.com",
	///         "phoneNumber": "0973614582"
	///     }
	///
	/// </remarks>
	/// <response code="201">Returns if an owner is created successfully</response>
	/// <response code="400">Returns if the validations are not passed or the operation fails</response>
	[Consumes(MediaTypeNames.Application.Json)]
	[Produces(MediaTypeNames.Application.Json)]
	[ProducesResponseType(typeof(OwnerDto), StatusCodes.Status201Created)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<OwnerDto>> Post([FromBody] OwnerDto createDto)
	{
		var readDto = await _ownersService.CreateAsync(createDto);
		return CreatedAtAction(nameof(Get), new { Key = readDto.Id }, readDto);
	}

	/// <summary>
	/// Updates an owner by the specified key
	/// </summary>
	/// <param name="key">An owner's identifier</param>
	/// <param name="updateDto">An owner modification DTO</param>
	/// <returns>No content</returns>
	/// <remarks>
	/// Sample request:
	///
	///    PUT /odata/v1/owners(43e789b0-5087-444d-a8c2-39fd1c5e4844)
	///    {
	///        "firstName": "Ryuji",
	///        "lastName": "Sakamoto",
	///        "age": 16,
	///        "email": "skull@ukr.net",
	///        "phoneNumber": "+380991684527"
	///    }
	///
	/// </remarks>
	/// <response code="204">Returns if an owner is updated successfully</response>
	/// <response code="400">Returns if the validations are not passed or the operation fails</response>
	/// <response code="404">Returns if an owner does not exist</response>
	[Consumes(MediaTypeNames.Application.Json)]
	[Produces(MediaTypeNames.Application.Json)]
	[ProducesResponseType(typeof(OwnerDto), StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
	public async Task<ActionResult> Put([FromODataUri] Guid key, [FromBody] OwnerDto updateDto)
	{
		await _ownersService.UpdateAsync(key, updateDto);
		return NoContent();
	}

	/// <summary>
	/// Updates an owner by the specified key
	/// </summary>
	/// <param name="key">An owner's identifier</param>
	/// <param name="delta">A delta that tracks changes for the modification DTO</param>
	/// <returns>No content</returns>
	/// <remarks>
	/// Sample request:
	///
	///    PATCH /odata/v1/owners(b4323fbc-87b1-4ae8-af75-92b3e16ede1c)
	///    {
	///        "firstName": "Yuji",
	///        "age": 15
	///    }
	///
	/// </remarks>
	/// <response code="204">Returns if an owner is updated successfully</response>
	/// <response code="400">Returns if the validations are not passed or the operation fails</response>
	/// <response code="404">Returns if an owner does not exist</response>
	[Consumes(MediaTypeNames.Application.Json)]
	[Produces(MediaTypeNames.Application.Json)]
	[ProducesResponseType(typeof(OwnerDto), StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
	public async Task<ActionResult> Patch([FromODataUri] Guid key, [FromBody] Delta<OwnerDto> delta)
	{
		await _ownersService.UpdateAsync(key, delta);
		return NoContent();
	}

	/// <summary>
	/// Deletes an owner by the specified key
	/// </summary>
	/// <param name="key">An owner's identifier</param>
	/// <returns>No content</returns>
	/// <response code="204">Returns if an owner is updated successfully</response>
	/// <response code="400">Returns if the operation fails</response>
	/// <response code="404">Returns if an owner does not exist</response>
	[Consumes(MediaTypeNames.Application.Json)]
	[Produces(MediaTypeNames.Application.Json)]
	[ProducesResponseType(typeof(OwnerDto), StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
	public async Task<ActionResult> Delete([FromODataUri] Guid key)
	{
		await _ownersService.DeleteAsync(key);
		return NoContent();
	}
}
