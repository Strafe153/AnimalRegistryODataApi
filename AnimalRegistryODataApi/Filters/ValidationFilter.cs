using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;
using ValidationException = Application.Exceptions.ValidationException;

namespace AnimalRegistryODataApi.Filters;

public class ValidationFilter : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument is null)
            {
                continue;
            }

            var validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());
            var validator = context.HttpContext.RequestServices.GetService(validatorType) as IValidator;

            if (validator is not null)
            {
                ValidationContext<object> validationContext = new(argument);
                var validationResult = await validator.ValidateAsync(validationContext);

                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult);
                }
            }
        }

        await next();
    }
}