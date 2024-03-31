using Application.DTOs;
using Microsoft.AspNetCore.OData.Deltas;

namespace Application.Services.Interfaces;

public interface IAnimalsService
{
	IQueryable<AnimalDto> GetAll();
	IQueryable<AnimalDto> GetById(Guid id);
	Task<AnimalDto> CreateAsync(AnimalDto dto);
	Task UpdateAsync(Guid id, AnimalDto dto);
	Task UpdateAsync(Guid id, Delta<AnimalDto> delta);
	Task DeleteAsync(Guid id);
}
