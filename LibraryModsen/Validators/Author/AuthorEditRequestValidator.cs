using FluentValidation;
using LibraryModsen.Application.Contracts.Author;

namespace LibraryModsen.Validators.Book;

public class AuthorEditRequestValidator : AbstractValidator<AuthorEditRequest>
{
    public AuthorEditRequestValidator()
    {
        RuleFor(r => r.Id).NotEmpty().Must(g => Guid.TryParse(g.ToString(), out _)).WithMessage("Guid id required");
        RuleFor(r => r.Name).NotEmpty().MinimumLength(1);
        RuleFor(r => r.Surname).NotEmpty().MinimumLength(1).Must(s => s.All(char.IsLetter));
        RuleFor(r => r.DateOfBirth).Must(d => !d.Equals(default(DateTime))).LessThan(DateTime.Now);
        RuleFor(r => r.Country).NotEmpty().MinimumLength(2);
    }
}
