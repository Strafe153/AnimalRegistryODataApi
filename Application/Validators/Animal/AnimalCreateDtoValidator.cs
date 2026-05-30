using Application.DTOs.Animal;
using FluentValidation;

namespace Application.Validators.Animal;

public class AnimalCreateDtoValidator : AbstractValidator<AnimalCreateDto>
{
	public AnimalCreateDtoValidator()
	{
		RuleFor(a => a.PetName)
			.NotEmpty()
			.WithMessage("Pet Name is required")
			.MinimumLength(2)
			.WithMessage("Pet Name must be at least 2 characters long")
			.MaximumLength(25)
			.WithMessage("Pet Name must not be longer than 25 characters");

		RuleFor(a => a.Kind)
			.NotEmpty()
			.WithMessage("Kind is required")
			.MinimumLength(1)
			.WithMessage("Kind must be at least 1 characters long")
			.MaximumLength(50)
			.WithMessage("Kind must not be longer than 50 characters");

		RuleFor(a => (int)a.Age)
			.GreaterThan(0)
			.WithMessage("Age must be greater than 0")
			.LessThan(50)
			.WithMessage("Age must be less than 50");

		RuleFor(a => a.OwnerId)
			.NotEmpty()
			.WithMessage("Owner Id is required");
	}
}
