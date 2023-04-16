using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace AnimalRegistryODataApi.Controllers;

public class OwnersController : ODataController
{
    private readonly IService<Owner, Guid> _ownersService;

	public OwnersController(IService<Owner, Guid> ownersService)
	{
		_ownersService = ownersService;
	}

	[EnableQuery(PageSize = 5)]
	public async Task<ActionResult<IEnumerable<Owner>>> Get() =>
		Ok(await _ownersService.GetAllAsync());

	[EnableQuery]
	public async Task<ActionResult<Owner>> Get([FromODataUri] Guid key) =>
		await _ownersService.GetByIdAsync(key);

	[EnableQuery]
	public async Task<ActionResult<Owner>> Post([FromBody] Owner owner)
	{
		await _ownersService.CreateAsync(owner);



		return CreatedAtAction(nameof(Owner), new { Key = owner.Id }, owner) ;
	}
}
