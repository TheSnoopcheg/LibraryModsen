using FluentValidation;
using LibraryModsen.Application.Contracts;

namespace LibraryModsen.Validators;

public class FilterRequestValidator : AbstractValidator<FilterRequest>
{
    public FilterRequestValidator()
    {
        RuleFor(r => r).NotNull().Must(CheckFilter);
    }
    private bool CheckFilter(FilterRequest filterRequest)
    {
        if (filterRequest.FilterType == Application.Common.FilterType.Author)
        {
            return Guid.TryParse(filterRequest.Data, out _);
        }
        else
            return true;
    }
}
