namespace Core.Entities;

public class Owner
{
    public Guid Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int Age { get; set; }

    public IList<Animal> Animals { get; set; }
}
