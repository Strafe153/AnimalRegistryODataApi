using Application.DTOs.Animal;
using Microsoft.AspNetCore.OData.Deltas;

namespace Application.Services.Interfaces;

public interface IAnimalsService
{
	IQueryable<AnimalReadDto> GetAll();
	IQueryable<AnimalReadDto> GetById(Guid id);
	Task<AnimalReadDto> CreateAsync(AnimalCreateDto dto);
	Task UpdateAsync(Guid id, AnimalUpdateDto dto);
	Task UpdateAsync(Guid id, Delta<AnimalUpdateDto> delta);
	Task DeleteAsync(Guid id);
}
