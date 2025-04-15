using LibraryModsen.Domain.Models;

namespace LibraryModsen.Application.Abstractions.Repositories;

public interface IBooksRepository
{
    Task<Guid> Add(Book book, List<Guid> authorIds);
    Task<bool> Any(Guid id);
    Task<Guid> DeleteById(Guid id);
    Task<Guid> Update(Book book, List<Guid> authorsIds);
    Task<IEnumerable<Book>> GetAll(int type, string fvalue);
    Task<IEnumerable<Book>> GetBooksByAuthor(Guid authorId);
    Task<Book?> GetById(Guid id);
    Task<Book?> GetByISBN(string iSBN);
    Task<IEnumerable<Book>> GetByTitle(string title);
    Task<IEnumerable<Book>> GetPage(int n, int size, int type, string fvalue);
}