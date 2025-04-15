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

    public async Task<int> GetAvailableNumOfBooks(Guid bookId)
    {
        if (!await _repository.AnyByBook(bookId))
        {
            return -1;
        }
        return await _repository.GetAvailableNumByBook(bookId);
    }

    public async Task AddBooks(Guid bookId, int num)
    {
        var books = Enumerable.Range(0, num).Select(b => new BookState
        {
            Id = Guid.NewGuid(),
            IsTaken = false,
            BookId = bookId
        });
        await _repository.AddRange(books);
    }

    public async Task<Guid> ExtendBook(Guid bookId, Guid userId, int days)
    {
        var book = await _repository.GetById(bookId);
        if (book == null || book.HolderId != userId)
        {
            return Guid.Empty;
        }
        book.ExpirationTime = book.ExpirationTime.AddDays(days);
        return await _repository.Update(book);
    }

    public async Task<Guid> ReturnBook(Guid bookId, Guid userId)
    {
        var book = await _repository.GetById(bookId);
        if (book == null || book.HolderId != userId)
        {
            return Guid.Empty;
        }
        book.IsTaken = false;
        book.HolderId = null;
        book.TakingTime = DateTime.MinValue;
        book.ExpirationTime = DateTime.MinValue;
        return await _repository.Update(book);
    }

    public async Task<Guid> Delete(Guid bookStateId)
    {
        if (!await _repository.Any(bookStateId))
            return Guid.Empty;
        return await _repository.Delete(bookStateId);
    }

    public async Task<Guid> TakeBook(Guid userId, Guid bookId)
    {
        var book = await _repository.GetFirstAvalaibleByBook(bookId);
        if (book == null)
        {
            return Guid.Empty;
        }
        book.IsTaken = true;
        book.HolderId = userId;
        book.TakingTime = DateTime.UtcNow;
        book.ExpirationTime = DateTime.UtcNow.AddDays(7);
        await _repository.Update(book);
        return book.Id;
    }
}
