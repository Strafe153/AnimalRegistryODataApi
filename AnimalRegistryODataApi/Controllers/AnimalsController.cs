using Application.Services;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace AnimalRegistryODataApi.Controllers;

public class AnimalsController : ODataController
{
    private readonly IService<AnimalDto> _animalsService;

    public AnimalsController(IService<AnimalDto> animalsService)
    {
        _animalsService = animalsService;
    }

    [EnableQuery(PageSize = 10)]
    public ActionResult<IQueryable<AnimalDto>> Get() =>
        Ok(_animalsService.GetAllAsync());

    [EnableQuery]
    public ActionResult<SingleResult<AnimalDto>> Get([FromODataUri] Guid key) =>
        Ok(SingleResult.Create(_animalsService.GetByIdAsync(key)));

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

    public async Task<ActionResult> Delete([FromODataUri] Guid key)
    {
        await _animalsService.DeleteAsync(key);
        return NoContent();
    }
}
