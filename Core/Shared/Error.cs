namespace Core.Shared;

public class Error
{
    public string Property { get; set; } = default!;
    public IEnumerable<string>? ErrorMessages { get; set; }
}
