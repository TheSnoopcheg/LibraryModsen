using FluentValidation;
using LibraryModsen.Application.Contracts.Book;

namespace LibraryModsen.Validators.Book;

public class BookCreationRequestValidator : AbstractValidator<BookCreationRequest>
{
    public BookCreationRequestValidator()
    {
        RuleFor(r => r.ISBN).SetValidator(new ISBNValidator());
        RuleFor(r => r.Title).NotEmpty().MinimumLength(1);
        RuleFor(r => r.Genre).NotEmpty().MinimumLength(1).Must(s => s.All(char.IsLetter));
        RuleForEach(r => r.AuthorsId).NotEmpty().Must(g => Guid.TryParse(g.ToString(), out _)).WithMessage("Guid id required");
        RuleFor(r => r.NumOfBooks).GreaterThan(0);
    }
}