using Application.DTOs.Requests;
using FluentValidation;

namespace Application.Validators;

public class UpdateBookRequestValidator : AbstractValidator<UpdateBookRequest>
{
    public UpdateBookRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.").When(x => x.Title != null)
            .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

        RuleFor(x => x.Author)
            .NotEmpty().WithMessage("Author is required.").When(x => x.Author != null)
            .MaximumLength(100).WithMessage("Author must not exceed 100 characters.");

        RuleFor(x => x.Year)
            .NotEmpty().WithMessage("Year is required.").When(x => x.Year != null)
            .InclusiveBetween(1, DateTime.Now.Year).WithMessage($"Year must be between 1 and {DateTime.Now.Year}.");
    }
}