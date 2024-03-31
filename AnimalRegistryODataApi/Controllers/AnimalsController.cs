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

namespace AnimalRegistryODataApi.Controllers;

[EnableRateLimiting(RateLimitingConstants.TokenBucket)]
public class AnimalsController : ODataController
{
	private readonly IAnimalsService _animalsService;

	public AnimalsController(IAnimalsService animalsService)
	{
		_animalsService = animalsService;
	}

	[EnableQuery(PageSize = 10)]
	[ResponseCache(CacheProfileName = CacheConstants.Default)]
	public ActionResult<IQueryable<AnimalDto>> Get() =>
		Ok(_animalsService.GetAll());

	[EnableQuery]
	[ResponseCache(CacheProfileName = CacheConstants.Default)]
	public ActionResult<SingleResult<AnimalDto>> Get([FromODataUri] Guid key) =>
		Ok(SingleResult.Create(_animalsService.GetById(key)));

	public async Task<ActionResult<AnimalDto>> Post([FromBody] AnimalDto createDto)
	{
		var readDto = await _animalsService.CreateAsync(createDto);
		return CreatedAtAction(nameof(Get), new { Key = readDto.Id }, readDto);
	}

	public async Task<ActionResult> Put([FromODataUri] Guid key, [FromBody] AnimalDto updateDto)
	{
		await _animalsService.UpdateAsync(key, updateDto);
		return NoContent();
	}

	public async Task<ActionResult> Patch([FromODataUri] Guid key, [FromBody] Delta<AnimalDto> delta)
	{
		await _animalsService.UpdateAsync(key, delta);
		return NoContent();
	}

	public async Task<ActionResult> Delete([FromODataUri] Guid key)
	{
		await _animalsService.DeleteAsync(key);
		return NoContent();
	}
}
