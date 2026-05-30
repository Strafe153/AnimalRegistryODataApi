using Application.DTOs.Owner;

namespace Application.DTOs.Animal;

public class AnimalReadDto
{
    public Guid Id { get; set; }
    public required string PetName { get; set; }
    public required string Kind { get; set; }
    public byte Age { get; set; }
    public OwnerReadDto Owner { get; set; } = default!;
}