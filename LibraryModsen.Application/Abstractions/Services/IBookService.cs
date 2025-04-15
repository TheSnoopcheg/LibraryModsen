using LibraryModsen.Application.Contracts;
using LibraryModsen.Application.Contracts.Book;

namespace LibraryModsen.Application.Abstractions.Services
{
    public interface IBookService
    {
        Task<bool> Any(Guid id);
        Task<Guid> CreateBook(BookCreationRequest request, string coverLink);
        Task<Guid> DeleteBook(Guid id);
        Task<Guid> EditBook(BookEditRequest request, string coverLink);
        Task<IEnumerable<BookFullResponse>> GetAll(FilterRequest request);
        Task<BookFullResponse?> GetById(Guid id);
        Task<BookFullResponse?> GetByISBN(string isbn);
        Task<IEnumerable<BookFullResponse>> GetByTitle(string title);
        Task<IEnumerable<BookFullResponse>> GetPage(int page, int size, FilterRequest request);
    }
}