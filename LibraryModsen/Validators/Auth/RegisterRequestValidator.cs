using FluentValidation;
using LibraryModsen.Application.Contracts.Auth;

namespace LibraryModsen.Validators.Auth;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(r => r.Email).EmailAddress();
        RuleFor(r => r.Password).MinimumLength(8);
        RuleFor(r => r.UserName).NotEmpty().MinimumLength(2);
    }
}
