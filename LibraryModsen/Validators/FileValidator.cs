using FluentValidation;

namespace LibraryModsen.Validators;

public class FileValidator : AbstractValidator<IFormFile>
{
    public FileValidator()
    {
        RuleFor(f => f.Length)
            .GreaterThan(0).WithMessage("File cannot be null")
            .LessThan(2097152).WithMessage("The max file size is 2 MB");
    }
}
