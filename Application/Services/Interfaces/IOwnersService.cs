using Application.DTOs.Owner;
using Microsoft.AspNetCore.OData.Deltas;

namespace Application.Services.Interfaces;

public interface IOwnersService
{
	IQueryable<OwnerReadDto> GetAll();
	IQueryable<OwnerReadDto> GetById(Guid id);
	Task<OwnerReadDto> CreateAsync(OwnerCreateDto dto);
	Task UpdateAsync(Guid id, OwnerUpdateDto dto);
	Task UpdateAsync(Guid id, Delta<OwnerUpdateDto> delta);
	Task DeleteAsync(Guid id);
}
