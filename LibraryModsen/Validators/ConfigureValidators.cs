using LibraryModsen.Validators.Auth;
using LibraryModsen.Validators.Author;
using LibraryModsen.Validators.Book;

namespace LibraryModsen.Validators;
public static class ServiceExtensions
{
    public static void ConfigureValidators(this IServiceCollection services) 
    {
        services.AddScoped<ISBNValidator>();
        services.AddScoped<FileValidator>();
        
        services.AddScoped<BookCreationRequestValidator>();
        services.AddScoped<BookEditRequestValidator>();

        services.AddScoped<AuthorCreationRequestValidator>();
        services.AddScoped<AuthorEditRequestValidator>();

        services.AddScoped<RegisterRequestValidator>();
        services.AddScoped<LoginRequestValidator>();
    }
}