using LibraryModsen.Application.Contracts.Author;
using LibraryModsen.Application.Contracts.Book;

namespace LibraryModsen.Application.Abstractions.Services
{
    public interface IAuthorService
    {
        Task<bool> Any(Guid id, CancellationToken cancelToken = default);
        Task CreateAuthor(AuthorCreationRequest request, CancellationToken cancelToken = default);
        Task DeleteAuthor(Guid id, CancellationToken cancelToken = default);
        Task EditAuthor(AuthorEditRequest request, CancellationToken cancelToken = default);
        Task<IEnumerable<AuthorFullResponse>> GetAll(CancellationToken cancelToken = default);
        Task<IEnumerable<BookFullResponse>> GetBooks(Guid id, CancellationToken cancelToken = default);
        Task<AuthorFullResponse?> GetById(Guid id, CancellationToken cancelToken = default);
        Task<IEnumerable<AuthorFullResponse>> GetPage(int page, int size, CancellationToken cancelToken = default);
    }
}