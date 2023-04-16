namespace Core.Entities;

public class Animal
{
    public Guid Id { get; set; }
    public string? PetName { get; set; }
    public string? Kind { get; set; }
    public int Age { get; set; }

    public Owner Owner { get; set; }
    public Guid OwnerId { get; set; }
}
