using FluentValidation;
using LibraryModsen.Application.Contracts.Author;

namespace LibraryModsen.Validators.Author;

public class AuthorCreationRequestValidator : AbstractValidator<AuthorCreationRequest>
{
    public AuthorCreationRequestValidator()
    {
        RuleFor(r => r.Name).NotEmpty().MinimumLength(1);
        RuleFor(r => r.Surname).NotEmpty().MinimumLength(1).Must(s => s.All(char.IsLetter));
        RuleFor(r => r.DateOfBirth).Must(d => !d.Equals(default(DateTime))).LessThan(DateTime.Now);
        RuleFor(r => r.Country).NotEmpty().MinimumLength(2);
    }
}