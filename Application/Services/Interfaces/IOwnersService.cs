using Application.DTOs;
using Microsoft.AspNetCore.OData.Deltas;

namespace Application.Services.Interfaces;

public interface IOwnersService
{
	IQueryable<OwnerDto> GetAll();
	IQueryable<OwnerDto> GetById(Guid id);
	Task<OwnerDto> CreateAsync(OwnerDto dto);
	Task UpdateAsync(Guid id, OwnerDto dto);
	Task UpdateAsync(Guid id, Delta<OwnerDto> delta);
	Task DeleteAsync(Guid id);
}
