using Application.DTOs.Queries;
using FluentValidation;

namespace Application.Validators;

public class LoanQueryParametersValidator : AbstractValidator<LoanQueryParameters>
{
    public LoanQueryParametersValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);
    }
}