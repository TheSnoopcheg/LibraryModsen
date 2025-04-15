using LibraryModsen.Application.Abstractions.Repositories;
using LibraryModsen.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryModsen.Persistence;

public static class ServiceExtensions
{
    public static void ConfigureRepositories(this IServiceCollection services)
    {
        services.AddScoped<IBooksRepository, BooksRepository>();
        services.AddScoped<IAuthorsRepository, AuthorsRepository>();
        services.AddScoped<IFilesRepository, FilesRepository>();
        services.AddScoped<IBookStatesRepository, BookStatesRepository>();
    }
}