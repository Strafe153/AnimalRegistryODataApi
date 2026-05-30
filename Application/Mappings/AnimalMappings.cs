using Application.DTOs.Animal;
using Domain.Entities;

namespace Application.Mappings;

public static class AnimalMappings
{
    public static AnimalReadDto ToReadDto(this Animal animal, bool mapOwner = true) => new()
    {
        Id = animal.Id,
        PetName = animal.PetName,
        Kind = animal.Kind,
        Age = animal.Age,
        Owner = mapOwner
            ? animal.Owner.ToReadDto(false) 
            : null!
    };

    public static AnimalUpdateDto ToUpdateDto(this Animal animal) => new()
    {
        PetName = animal.PetName,
        Kind = animal.Kind,
        Age = animal.Age,
        OwnerId = animal.Owner.Id
    };

    public static Animal ToAnimal(this AnimalCreateDto dto) =>
        new()
        {
            PetName = dto.PetName,
            Kind = dto.Kind,
            Age = dto.Age
        };

    public static void Update(this AnimalUpdateDto dto, Animal animal)
    {
        animal.PetName = dto.PetName;
        animal.Kind = dto.Kind;
        animal.Age = dto.Age;
    }
}