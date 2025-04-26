using LibraryModsen.Domain.Models;

namespace LibraryModsen.Application.Abstractions.Repositories;

public interface IAuthorsRepository : IBaseRepository<Author>
{
    Task<bool> Any(Guid id, CancellationToken cancelToken = default);
    Task<IEnumerable<Book>> GetAuthorBooks(Guid authorId, CancellationToken cancelToken = default);
    Task<IEnumerable<Author>> GetPage(int n, int size, CancellationToken cancelToken = default);
}