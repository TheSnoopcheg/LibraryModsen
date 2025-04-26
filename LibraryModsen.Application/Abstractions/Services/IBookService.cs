using LibraryModsen.Application.Contracts;
using LibraryModsen.Application.Contracts.Book;

namespace LibraryModsen.Application.Abstractions.Services
{
    public interface IBookService
    {
        Task<bool> Any(Guid id, CancellationToken cancelToken = default);
        Task CreateBook(BookCreationRequest request, string coverLink, CancellationToken cancelToken = default);
        Task DeleteBook(Guid id, CancellationToken cancelToken = default);
        Task EditBook(BookEditRequest request, string coverLink, CancellationToken cancelToken = default);
        Task<IEnumerable<BookFullResponse>> GetAll(FilterRequest request, CancellationToken cancelToken = default);
        Task<BookFullResponse?> GetById(Guid id, CancellationToken cancelToken = default);
        Task<BookFullResponse?> GetByISBN(string isbn, CancellationToken cancelToken = default);
        Task<IEnumerable<BookFullResponse>> GetByTitle(string title, CancellationToken cancelToken = default);
        Task<IEnumerable<BookFullResponse>> GetPage(int page, int size, FilterRequest request, CancellationToken cancelToken = default);
    }
}