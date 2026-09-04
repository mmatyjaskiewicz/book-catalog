using Application.DTOs.Requests;
using FluentValidation;

namespace Application.Validators;

public class UpdateAuthorRequestValidator : AbstractValidator<UpdateAuthorRequest>
{
    public UpdateAuthorRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.")
            .When(x => x.Name != null);
    }
}