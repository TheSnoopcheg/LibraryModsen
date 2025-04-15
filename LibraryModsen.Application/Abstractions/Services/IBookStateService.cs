namespace LibraryModsen.Application.Abstractions.Services
{
    public interface IBookStateService
    {
        Task AddBooks(Guid bookId, int num);
        Task<Guid> Delete(Guid bookStateId);
        Task<Guid> ExtendBook(Guid bookId, Guid userId, int days);
        Task<int> GetAvailableNumOfBooks(Guid bookId);
        Task<Guid> ReturnBook(Guid bookId, Guid userId);
        Task<Guid> TakeBook(Guid userId, Guid bookId);
    }
}