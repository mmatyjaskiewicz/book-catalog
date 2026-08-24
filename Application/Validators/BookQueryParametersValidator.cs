using Application.DTOs.Queries;
using FluentValidation;

namespace Application.Validators;

public class BookQueryParametersValidator : AbstractValidator<BookQueryParameters>
{
    public BookQueryParametersValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);
    }
}