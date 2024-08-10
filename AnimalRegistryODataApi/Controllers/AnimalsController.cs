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

namespace AnimalRegistryODataApi.Controllers;

[EnableRateLimiting(RateLimitingConstants.TokenBucket)]
public class AnimalsController : ODataController
{
	private readonly IAnimalsService _animalsService;

	public AnimalsController(IAnimalsService animalsService)
	{
		_animalsService = animalsService;
	}

	/// <summary>
	/// Fetches a collection of animals
	/// </summary>
	/// <returns>A collection of animals</returns>
	/// <response code="200">Returns a collection of animals</response>
	[EnableQuery(PageSize = 10)]
	[ResponseCache(CacheProfileName = CacheConstants.Default)]
	[Consumes(MediaTypeNames.Application.Json)]
	[Produces(MediaTypeNames.Application.Json)]
	[ProducesResponseType(typeof(IQueryable<AnimalDto>), StatusCodes.Status200OK)]
	public ActionResult<IQueryable<AnimalDto>> Get() => Ok(_animalsService.GetAll());

	/// <summary>
	/// Fetches an animal by the specified key
	/// </summary>
	/// <param name="key">An animal's identifier</param>
	/// <returns>An animal by the specified key</returns>
	/// <response code="200">Returns an animal by the specified identifier</response>
	[EnableQuery]
	[ResponseCache(CacheProfileName = CacheConstants.Default)]
	[Consumes(MediaTypeNames.Application.Json)]
	[Produces(MediaTypeNames.Application.Json)]
	[ProducesResponseType(typeof(SingleResult<AnimalDto>), StatusCodes.Status200OK)]
	public ActionResult<SingleResult<AnimalDto>> Get([FromODataUri] Guid key) =>
		Ok(SingleResult.Create(_animalsService.GetById(key)));

	/// <summary>
	/// Creates an animal
	/// </summary>
	/// <param name="createDto">An animal creation DTO</param>
	/// <returns>The newly created animal</returns>
	/// <remarks>
	/// Sample request:
	///
	///     POST /odata/v1/animals
	///     {
	///         "petName": "Morgana",
	///         "kind": "Not a cat",
	///         "age": 2,
	///         "ownerId": "715b112e-4804-4335-9eda-7da6dc60636d"
	///     }
	///
	/// </remarks>
	/// <response code="201">Returns if an animal is created successfully</response>
	/// <response code="400">Returns if the validations are not passed or the operation fails</response>
	[Consumes(MediaTypeNames.Application.Json)]
	[Produces(MediaTypeNames.Application.Json)]
	[ProducesResponseType(typeof(AnimalDto), StatusCodes.Status201Created)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<AnimalDto>> Post([FromBody] AnimalDto createDto)
	{
		var readDto = await _animalsService.CreateAsync(createDto);
		return CreatedAtAction(nameof(Get), new { Key = readDto.Id }, readDto);
	}

	/// <summary>
	/// Updates an animal by the specified key
	/// </summary>
	/// <param name="key">An animal's identifier</param>
	/// <param name="updateDto">An animal modification DTO</param>
	/// <returns>No content</returns>
	/// <remarks>
	/// Sample request:
	///
	///    PUT /odata/v1/animals(f31a113c-6c81-4e31-83c5-bed07ba9dfc0)
	///    {
	///        "petName": "Junky",
	///        "kind": "Stray dog",
	///        "age": 4,
	///        "ownerId": "c9fb65b0-4bd4-4ced-aef5-43def338fcf5"
	///    }
	///
	/// </remarks>
	/// <response code="204">Returns if an animal is updated successfully</response>
	/// <response code="400">Returns if the validations are not passed or the operation fails</response>
	/// <response code="404">Returns if an animal does not exist</response>
	[Consumes(MediaTypeNames.Application.Json)]
	[Produces(MediaTypeNames.Application.Json)]
	[ProducesResponseType(typeof(AnimalDto), StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
	public async Task<ActionResult> Put([FromODataUri] Guid key, [FromBody] AnimalDto updateDto)
	{
		await _animalsService.UpdateAsync(key, updateDto);
		return NoContent();
	}

	/// <summary>
	/// Updates an animal by the specified key
	/// </summary>
	/// <param name="key">An animal's identifier</param>
	/// <param name="delta">A delta that tracks changes for the modification DTO</param>
	/// <returns>No content</returns>
	/// <remarks>
	/// Sample request:
	///
	///    PATCH /odata/v1/animals(7acb6100-f565-4dfb-b3e4-74640cb09f57)
	///    {
	///        "kind": "Scottish fold",
	///        "age": 5
	///    }
	///
	/// </remarks>
	/// <response code="204">Returns if an animal is updated successfully</response>
	/// <response code="400">Returns if the validations are not passed or the operation fails</response>
	/// <response code="404">Returns if an animal does not exist</response>
	[Consumes(MediaTypeNames.Application.Json)]
	[Produces(MediaTypeNames.Application.Json)]
	[ProducesResponseType(typeof(AnimalDto), StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
	public async Task<ActionResult> Patch([FromODataUri] Guid key, [FromBody] Delta<AnimalDto> delta)
	{
		await _animalsService.UpdateAsync(key, delta);
		return NoContent();
	}

	/// <summary>
	/// Deletes an animal by the specified key
	/// </summary>
	/// <param name="key">An animal's identifier</param>
	/// <returns>No content</returns>
	/// <response code="204">Returns if an animal is updated successfully</response>
	/// <response code="400">Returns if the operation fails</response>
	/// <response code="404">Returns if an animal does not exist</response>
	[Consumes(MediaTypeNames.Application.Json)]
	[Produces(MediaTypeNames.Application.Json)]
	[ProducesResponseType(typeof(AnimalDto), StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
	public async Task<ActionResult> Delete([FromODataUri] Guid key)
	{
		await _animalsService.DeleteAsync(key);
		return NoContent();
	}
}
