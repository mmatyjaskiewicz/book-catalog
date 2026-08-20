using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Filters;

public class ValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument is null)
                continue;

            var validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());

            var validator = context.HttpContext.RequestServices.GetService(validatorType);

            if (validator is not IValidator nonGenericValidator)
                continue;

            ValidationResult result = await nonGenericValidator.ValidateAsync(new ValidationContext<object>(argument),
                context.HttpContext.RequestAborted);

            if (!result.IsValid)
            {
                var error = result.Errors
                    .Select(x => x.ErrorMessage)
                    .FirstOrDefault();

                context.Result = new BadRequestObjectResult(new
                {
                    Title = "Validation failed",
                    Status = StatusCodes.Status400BadRequest,
                    Error = error
                });

                return;
            }
        }

        await next();
    }
}