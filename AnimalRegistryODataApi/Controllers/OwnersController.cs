using Core.DTOs;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.RateLimiting;

namespace AnimalRegistryODataApi.Controllers;

[EnableRateLimiting("tokenBucket")]
[ResponseCache(CacheProfileName = "Default")]
public class OwnersController : ODataController
{
    private readonly IService<OwnerDto> _ownersService;

    public OwnersController(IService<OwnerDto> ownersService)
    {
        _ownersService = ownersService;
    }

    [EnableQuery(PageSize = 10)]
    public ActionResult<IQueryable<OwnerDto>> Get() =>
        Ok(_ownersService.GetAll());

    [EnableQuery]
    [ResponseCache(Duration = 30)]
    public ActionResult<SingleResult<OwnerDto>> Get([FromODataUri] Guid key) =>
        Ok(SingleResult.Create(_ownersService.GetById(key)));

    public async Task<ActionResult<OwnerDto>> Post([FromBody] OwnerDto createDto)
    {
        var readDto = await _ownersService.CreateAsync(createDto);
        return CreatedAtAction(nameof(Get), new { Key = readDto.Id }, readDto);
    }

    public async Task<ActionResult> Put([FromODataUri] Guid key, [FromBody] OwnerDto updateDto)
    {
        await _ownersService.UpdateAsync(key, updateDto);
        return NoContent();
    }

    public async Task<ActionResult> Patch([FromODataUri] Guid key, [FromBody] Delta<OwnerDto> delta)
    {
        await _ownersService.UpdateAsync(key, delta);
        return NoContent();
    }

    public async Task<ActionResult> Delete([FromODataUri] Guid key)
    {
        await _ownersService.DeleteAsync(key);
        return NoContent();
    }
}
