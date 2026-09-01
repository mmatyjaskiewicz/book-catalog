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
        
        RuleFor(x => x.AuthorId)
            .NotEmpty().WithMessage("AuthorId cannot be empty.")
            .When(x => x.AuthorId.HasValue);


        RuleFor(x => x.PublishYear)
            .NotEmpty().WithMessage("Year is required.").When(x => x.PublishYear != null)
            .InclusiveBetween(1, DateTime.Now.Year).WithMessage($"Year must be between 1 and {DateTime.Now.Year}.");
    }
}