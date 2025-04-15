using LibraryModsen.Application.Abstractions.Services;
using LibraryModsen.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryModsen.Application;

public static class ServiceExtensions
{
    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IAuthorService, AuthorService>();
        services.AddScoped<IFilesService, FilesService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IBookStateService, BookStateService>();
        services.AddScoped<IUserService, UserService>();
    }
}

