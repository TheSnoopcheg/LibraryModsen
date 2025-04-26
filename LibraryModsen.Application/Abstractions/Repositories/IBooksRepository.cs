using LibraryModsen.Domain.Models;

namespace LibraryModsen.Application.Abstractions.Repositories;

public interface IBooksRepository : IBaseRepository<Book>
{
    Task<bool> Any(Guid id, CancellationToken cancellationToken = default);
    Task<Guid> Add(Book book, List<Guid> authorIds, CancellationToken cancelToken = default);
    Task<IEnumerable<Book>> GetAll(int type, string fvalue, CancellationToken cancelToken = default);
    Task<IEnumerable<Book>> GetBooksByAuthor(Guid authorId, CancellationToken cancelToken = default);
    Task<Guid> Update(Book book, List<Guid> authorsIds, CancellationToken cancelToken = default);
    Task<Book?> GetByISBN(string iSBN, CancellationToken cancellationToken = default);
    Task<IEnumerable<Book>> GetByTitle(string title, CancellationToken cancelToken = default);
    Task<IEnumerable<Book>> GetPage(int n, int size, int type, string fvalue, CancellationToken cancelToken = default);
}