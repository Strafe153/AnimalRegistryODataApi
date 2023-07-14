using Core.DTOs;
using FluentValidation;

namespace AnimalRegistryODataApi.Validators;

public class AnimalValidator : AbstractValidator<AnimalDto>
{
    public AnimalValidator()
    {
        RuleFor(o => o.PetName)
            .NotEmpty()
            .WithMessage("Pet Name is required")
            .MinimumLength(2)
            .WithMessage("Pet Name must be at least 2 characters long")
            .MaximumLength(25)
            .WithMessage("Pet Name must not be longer than 25 characters");

        RuleFor(o => o.Kind)
            .NotEmpty()
            .WithMessage("Kind is required")
            .MinimumLength(1)
            .WithMessage("Kind must be at least 1 characters long")
            .MaximumLength(50)
            .WithMessage("Kind must not be longer than 50 characters");

        RuleFor(o => (int)o.Age)
            .GreaterThan(0)
            .WithMessage("Age must be greater than 0")
            .LessThan(50)
            .WithMessage("Age must be less than 50");

        RuleFor(o => o.OwnerId)
            .NotEmpty()
            .WithMessage("Owner Id is required");
    }
}
