using FluentValidation;
using LibraryModsen.Application.Contracts;
using LibraryModsen.Application.Contracts.Auth;
using LibraryModsen.Application.Contracts.Author;
using LibraryModsen.Application.Contracts.Book;
using LibraryModsen.Validators.Auth;
using LibraryModsen.Validators.Author;
using LibraryModsen.Validators.Book;

namespace LibraryModsen.Validators;
public static class ServiceExtensions
{
    public static void ConfigureValidators(this IServiceCollection services) 
    {
        services.AddScoped<IValidator<string>, ISBNValidator>();
        services.AddScoped<IValidator<IFormFile>, FileValidator>();
        
        services.AddScoped<IValidator<BookCreationRequest>, BookCreationRequestValidator>();
        services.AddScoped<IValidator<BookEditRequest>, BookEditRequestValidator>();

        services.AddScoped<IValidator<AuthorCreationRequest>, AuthorCreationRequestValidator>();
        services.AddScoped<IValidator<AuthorEditRequest>, AuthorEditRequestValidator>();

        services.AddScoped<IValidator<RegisterRequest>, RegisterRequestValidator>();
        services.AddScoped<IValidator<LoginRequest>, LoginRequestValidator>();

        services.AddScoped<IValidator<FilterRequest>, FilterRequestValidator>();
    }
}