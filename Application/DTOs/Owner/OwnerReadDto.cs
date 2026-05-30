using Application.DTOs.Animal;

namespace Application.DTOs.Owner;

public class OwnerReadDto
{
    public Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public byte Age { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public List<AnimalReadDto> Animals { get; set; } = [];
}