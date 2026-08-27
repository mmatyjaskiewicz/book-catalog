using Application.Validators;
using FluentValidation;

namespace WebApi.Extensions;

public static class ValidationExtensions
{
    public static IServiceCollection AddValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<CreateBookRequestValidator>();

        return services;
    }
}