using LibraryModsen.Domain.Models;

namespace LibraryModsen.Application.Abstractions.Repositories;
public interface IBookStatesRepository
{
    Task<Guid> Add(BookState bookState);
    Task AddRange(IEnumerable<BookState> bookStates);
    Task<bool> Any(Guid id);
    Task<bool> AnyByBook(Guid bookId);
    Task<Guid> Delete(Guid id);
    Task<IEnumerable<BookState>> GetAll();
    Task<IEnumerable<BookState>> GetAllByBook(Guid id);
    Task<int> GetAvailableNumByBook(Guid id);
    Task<BookState?> GetById(Guid id);
    Task<BookState?> GetFirstAvalaibleByBook(Guid bookId);
    Task<int> GetNumByBook(Guid id);
    Task<Guid> Update(BookState bookState);
}