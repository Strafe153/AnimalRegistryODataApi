using Core.DTOs;
using Core.Shared;
using FluentValidation;
using System.Text.RegularExpressions;

namespace AnimalRegistryODataApi.Validators;

public class OwnerValidator : AbstractValidator<OwnerDto>
{
    public OwnerValidator()
    {
        RuleFor(o => o.FirstName)
            .NotEmpty()
            .WithMessage("First Name is required")
            .MinimumLength(2)
            .WithMessage("First Name must be at least 2 characters long")
            .MaximumLength(25)
            .WithMessage("First Name must not be longer than 25 characters");

        RuleFor(o => o.LastName)
            .NotEmpty()
            .WithMessage("Last Name is required")
            .MinimumLength(2)
            .WithMessage("Last Name must be at least 2 characters long")
            .MaximumLength(25)
            .WithMessage("Last Name must not be longer than 25 characters");

        RuleFor(o => (int)o.Age)
            .GreaterThan(14)
            .WithMessage("Age must be greater than 14")
            .LessThan(100)
            .WithMessage("Age must be less than 100");

        RuleFor(o => o.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Email must be in the proper format");

        RuleFor(vm => vm.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required")
            .MinimumLength(10)
            .WithMessage("Phone number length must be at least 10 characters long")
            .MaximumLength(20)
            .WithMessage("Phone number length must be less than 20 characters")
            .Matches(new Regex(ValidatorConstants.PhoneNumberPattern))
            .WithMessage("Incorrect phone number format");
    }
}
