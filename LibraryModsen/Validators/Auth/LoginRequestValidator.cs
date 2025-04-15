using FluentValidation;
using LibraryModsen.Application.Contracts.Auth;

namespace LibraryModsen.Validators.Auth;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(r => r.Email).EmailAddress();
        RuleFor(r => r.Password).MinimumLength(8);
    }
}
