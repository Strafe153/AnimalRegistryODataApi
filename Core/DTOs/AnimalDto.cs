namespace Core.DTOs;

public class AnimalDto
{
    public Guid Id { get; set; }
    public string? PetName { get; set; }
    public string? Kind { get; set; }
    public int Age { get; set; }
    public Guid OwnerId { get; set; }
    public OwnerDto? Owner { get; set; }
}
