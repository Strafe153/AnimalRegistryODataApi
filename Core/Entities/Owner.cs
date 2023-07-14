namespace Core.Entities;

public class Owner
{
    public virtual Guid Id { get; set; }
    public virtual string FirstName { get; set; } = default!;
    public virtual string LastName { get; set; } = default!;
    public virtual byte Age { get; set; }
    public virtual string Email { get; set; } = default!;
    public virtual string PhoneNumber { get; set; } = default!;

    public virtual IList<Animal> Animals { get; set; } = default!;
}
