using AutoMapper;
using LibraryModsen.Application.Abstractions.Repositories;
using LibraryModsen.Application.Abstractions.Services;
using LibraryModsen.Domain.Models;

namespace LibraryModsen.Application.Services;

public class BookStateService(
        IBookStatesRepository repository,
        IMapper mapper) : IBookStateService
{
    private readonly IBookStatesRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<int> GetAvailableNumOfBooks(Guid bookId, CancellationToken cancelToken = default)
    {
        if (!await _repository.AnyByBook(bookId, cancelToken))
            throw new Exception("No available books");
        return await _repository.GetAvailableNumByBook(bookId, cancelToken);
    }

    public async Task AddBooks(Guid bookId, int num, CancellationToken cancelToken = default)
    {
        if (num < 1)
            throw new Exception("num cannot be less than 1");
        var books = Enumerable.Range(0, num).Select(b => new BookState
        {
            Id = Guid.NewGuid(),
            IsTaken = false,
            BookId = bookId
        });
        await _repository.AddRange(books, cancelToken);
    }

    public async Task ExtendBook(Guid bookId, Guid userId, int days, CancellationToken cancelToken = default)
    {
        var book = await _repository.GetById(bookId, cancelToken);
        if (book == null)
            throw new Exception("Book not found");
        if (book.HolderId != userId)
            throw new Exception("Holder and user IDs aren't matching");
        book.ExpirationTime = book.ExpirationTime.AddDays(days);
        await _repository.Update(book, cancelToken);
    }

    public async Task ReturnBook(Guid bookId, Guid userId, CancellationToken cancelToken = default)
    {
        var book = await _repository.GetById(bookId, cancelToken);
        if (book == null)
            throw new Exception("Book not found");
        if (book.HolderId != userId)
            throw new Exception("Holder and user IDs aren't matching");
        book.IsTaken = false;
        book.HolderId = null;
        book.TakingTime = DateTime.MinValue;
        book.ExpirationTime = DateTime.MinValue;
        await _repository.Update(book, cancelToken);
    }

    public async Task Delete(Guid bookStateId, CancellationToken cancelToken = default)
    {
        if (!await _repository.Any(bookStateId, cancelToken))
            throw new Exception("Book not found");
        await _repository.Delete(bookStateId, cancelToken);
    }

    public async Task TakeBook(Guid userId, Guid bookId, CancellationToken cancelToken = default)
    {
        var book = await _repository.GetFirstAvalaibleByBook(bookId, cancelToken);
        if (book == null)
            throw new Exception("No available book");
        book.IsTaken = true;
        book.HolderId = userId;
        book.TakingTime = DateTime.UtcNow;
        book.ExpirationTime = DateTime.UtcNow.AddDays(7);
        await _repository.Update(book, cancelToken);
    }
}
