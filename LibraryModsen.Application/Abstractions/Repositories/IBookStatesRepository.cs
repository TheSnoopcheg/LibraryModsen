using LibraryModsen.Domain.Models;

namespace LibraryModsen.Application.Abstractions.Repositories;
public interface IBookStatesRepository : IBaseRepository<BookState>
{
    Task<bool> Any(Guid id, CancellationToken cancelToken = default);
    Task AddRange(IEnumerable<BookState> bookStates, CancellationToken cancelToken = default);
    Task<bool> AnyByBook(Guid bookId, CancellationToken cancelToken = default);
    Task<IEnumerable<BookState>> GetAllByBook(Guid id, CancellationToken cancelToken = default);
    Task<int> GetAvailableNumByBook(Guid id, CancellationToken cancelToken = default);
    Task<BookState?> GetFirstAvalaibleByBook(Guid bookId, CancellationToken cancelToken = default);
    Task<int> GetNumByBook(Guid id, CancellationToken cancelToken = default);
}