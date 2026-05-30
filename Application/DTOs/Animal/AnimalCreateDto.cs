namespace Application.DTOs.Animal;

public class AnimalCreateDto
{
    public string PetName { get; set; } = string.Empty;
    public string Kind { get; set; } = string.Empty;
    public byte Age { get; set; }
    public Guid OwnerId { get; set; }
}