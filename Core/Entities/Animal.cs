namespace Core.Entities;

public class Animal
{
    public virtual Guid Id { get; set; }
    public virtual string PetName { get; set; } = string.Empty;
    public virtual string Kind { get; set; } = string.Empty;
    public virtual byte Age { get; set; }

    public virtual Owner Owner { get; set; } = default!;
}
