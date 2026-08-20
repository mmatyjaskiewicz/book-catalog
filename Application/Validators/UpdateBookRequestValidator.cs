using Application.DTOs.Requests;
using FluentValidation;

namespace Application.Validators;

public class UpdateBookRequestValidator : AbstractValidator<UpdateBookRequest>
{
    public UpdateBookRequestValidator()
    {
        RuleFor(x => x.Title)
            .MaximumLength(100).WithMessage("Title must not exceed 100 characters.")
            .When(x => !string.IsNullOrEmpty(x.Title));

        RuleFor(x => x.Author)
            .MaximumLength(100).WithMessage("Author must not exceed 100 characters.")
            .When(x => !string.IsNullOrEmpty(x.Author));

        RuleFor(x => x.Year)
            .InclusiveBetween(1, DateTime.Now.Year).WithMessage($"Year must be between 1 and {DateTime.Now.Year}.")
            .When(x => x.Year.HasValue);
    }
}