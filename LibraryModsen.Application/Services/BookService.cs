using AutoMapper;
using LibraryModsen.Application.Abstractions.Repositories;
using LibraryModsen.Application.Abstractions.Services;
using LibraryModsen.Application.Contracts;
using LibraryModsen.Application.Contracts.Book;
using LibraryModsen.Domain.Models;

namespace LibraryModsen.Application.Services;

public class BookService(
        IBooksRepository repository,
        IMapper mapper) : IBookService
{
    private readonly IBooksRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    public async Task<bool> Any(Guid id)
    {
        return await _repository.Any(id);
    }

    public async Task<IEnumerable<BookFullResponse>> GetAll(FilterRequest request)
    {
        var books = await _repository.GetAll((int)request.FilterType, request.Data);
        return books.Select(b => _mapper.Map<BookFullResponse>(b));
    }

    public async Task<BookFullResponse?> GetById(Guid id)
    {
        var book = await _repository.GetById(id);
        return _mapper.Map<BookFullResponse?>(book);
    }

    public async Task<BookFullResponse?> GetByISBN(string isbn)
    {
        var book = await _repository.GetByISBN(isbn);
        return _mapper.Map<BookFullResponse?>(book);
    }

    public async Task<Guid> CreateBook(BookCreationRequest request, string coverLink)
    {
        var book = _mapper.Map<Book>(request);
        book.CoverLink = coverLink;
        book.Id = Guid.NewGuid();
        var id = await _repository.Add(book, request.AuthorsId);
        return id;
    }

    public async Task<Guid> EditBook(BookEditRequest request, string coverLink)
    {
        if (!await _repository.Any(request.Id))
            return Guid.Empty;

        var book = _mapper.Map<Book>(request);
        var dbBook = await _repository.GetById(request.Id);
        if (!string.IsNullOrEmpty(coverLink))
            book.CoverLink = coverLink;
        else
            book.CoverLink = dbBook!.CoverLink;
        var id = await _repository.Update(book, request.AuthorsId);
        return id;
    }
    public async Task<Guid> DeleteBook(Guid id)
    {
        if (!await _repository.Any(id))
            return Guid.Empty;
        return await _repository.DeleteById(id);
    }
    public async Task<IEnumerable<BookFullResponse>> GetPage(int page, int size, FilterRequest request)
    {
        var books = await _repository.GetPage(page, size, (int)request.FilterType, request.Data);
        return books.Select(b => _mapper.Map<BookFullResponse>(b));
    }
    public async Task<IEnumerable<BookFullResponse>> GetByTitle(string title)
    {
        var books = await _repository.GetByTitle(title);
        return books.Select(b => _mapper.Map<BookFullResponse>(b));
    }
}
