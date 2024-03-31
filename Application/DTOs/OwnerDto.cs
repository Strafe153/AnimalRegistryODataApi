namespace Application.DTOs;

public class OwnerDto
{
	public Guid Id { get; set; }
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public byte Age { get; set; }
	public string? Email { get; set; }
	public string? PhoneNumber { get; set; }

	public IList<AnimalDto>? Animals { get; set; }
}
