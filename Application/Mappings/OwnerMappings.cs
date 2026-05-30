using Application.DTOs.Owner;
using Domain.Entities;

namespace Application.Mappings;

public static class OwnerMappings
{
    public static OwnerReadDto ToReadDto(this Owner owner, bool mapAnimals = true) => new()
    {
        Id = owner.Id,
        FirstName = owner.FirstName,
        LastName = owner.LastName,
        Age = owner.Age,
        Email = owner.Email,
        PhoneNumber = owner.PhoneNumber,
        Animals = mapAnimals
            ? [.. owner.Animals.Select(o => o.ToReadDto(false))]
            : []
    };

    public static OwnerUpdateDto ToUpdateDto(this Owner owner) => new()
    {
        FirstName = owner.FirstName,
        LastName = owner.LastName,
        Age = owner.Age,
        Email = owner.Email,
        PhoneNumber = owner.PhoneNumber
    };

    public static Owner ToOwner(this OwnerCreateDto dto) =>
        new()
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Age = dto.Age,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber
        };

    public static void Update(this OwnerUpdateDto dto, Owner owner)
    {
        owner.FirstName = dto.FirstName;
        owner.LastName = dto.LastName;
        owner.Age = dto.Age;
        owner.Email = dto.Email;
        owner.PhoneNumber = dto.PhoneNumber;
    }
}