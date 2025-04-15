using LibraryModsen.Domain.Models;

namespace LibraryModsen.Application.Abstractions.Repositories;

public interface IAuthorsRepository
{
    Task<Guid> Add(Author author);
    Task<bool> Any(Guid id);
    Task<Guid> Delete(Guid id);
    Task<IEnumerable<Author>> GetAll();
    Task<IEnumerable<Book>> GetAuthorBooks(Guid authorId);
    Task<Author?> GetById(Guid id);
    Task<IEnumerable<Author>> GetPage(int n, int size);
    Task<Guid> Update(Author author);
}