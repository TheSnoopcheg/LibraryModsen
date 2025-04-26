namespace LibraryModsen.Application.Abstractions.Services
{
    public interface IBookStateService
    {
        Task AddBooks(Guid bookId, int num, CancellationToken cancelToken = default);
        Task Delete(Guid bookStateId, CancellationToken cancelToken = default);
        Task ExtendBook(Guid bookId, Guid userId, int days, CancellationToken cancelToken = default);
        Task<int> GetAvailableNumOfBooks(Guid bookId, CancellationToken cancelToken = default);
        Task ReturnBook(Guid bookId, Guid userId, CancellationToken cancelToken = default);
        Task TakeBook(Guid userId, Guid bookId, CancellationToken cancelToken = default);
    }
}