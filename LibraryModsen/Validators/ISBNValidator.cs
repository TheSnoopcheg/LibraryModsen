using FluentValidation;

namespace LibraryModsen.Validators;

public class ISBNValidator : AbstractValidator<string>
{
    public ISBNValidator()
    {
        RuleFor(x => x).Must(x => x.Length == 10 || x.Length == 13).WithMessage("The number of characters must be 10 or 13");
        RuleFor(x => x).Must(c => long.TryParse(c, out long _)).WithMessage("Only digits are allowed");
    }
}
