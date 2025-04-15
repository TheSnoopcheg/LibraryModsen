using LibraryModsen.Application.Contracts.Author;
using LibraryModsen.Application.Contracts.Book;

namespace LibraryModsen.Application.Abstractions.Services
{
    public interface IAuthorService
    {
        Task<bool> Any(Guid id);
        Task<Guid> CreateAuthor(AuthorCreationRequest request);
        Task<Guid> DeleteAuthor(Guid id);
        Task<Guid> EditAuthor(AuthorEditRequest request);
        Task<IEnumerable<AuthorFullResponse>> GetAll();
        Task<IEnumerable<BookFullResponse>> GetBooks(Guid id);
        Task<AuthorFullResponse?> GetById(Guid id);
        Task<IEnumerable<AuthorFullResponse>> GetPage(int page, int size);
    }
}