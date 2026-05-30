using FluentValidation.Results;

namespace Application.Exceptions;

public class ValidationException : Exception
{
    public Dictionary<string, List<string>> Errors { get; }

    public ValidationException(ValidationResult result) : base("Validation error")
    {
        Errors = result.Errors
            .GroupBy(f => f.PropertyName)
            .ToDictionary(
				g => g.Key,
				g => g.Select(f => f.ErrorMessage).ToList());
    }
}
