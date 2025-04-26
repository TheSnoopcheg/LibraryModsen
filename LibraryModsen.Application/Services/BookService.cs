using AutoMapper;
using LibraryModsen.Application.Abstractions.Repositories;
using LibraryModsen.Application.Abstractions.Services;
using LibraryModsen.Application.Contracts;
using LibraryModsen.Application.Contracts.Book;
using LibraryModsen.Domain.Models;

namespace LibraryModsen.Application.Services;

public class BookService(
        IBooksRepository repository,
        IBookStateService bookStateService,
        IMapper mapper) : IBookService
{
    private readonly IBooksRepository _repository = repository;
    private readonly IBookStateService _bookStateService = bookStateService;
    private readonly IMapper _mapper = mapper;

    public async Task<bool> Any(Guid id, CancellationToken cancelToken = default)
    {
        return await _repository.Any(id, cancelToken);
    }

    public async Task<IEnumerable<BookFullResponse>> GetAll(FilterRequest request, CancellationToken cancelToken = default)
    {
        var books = await _repository.GetAll((int)request.FilterType, request.Data, cancelToken);
        return books.Select(b => _mapper.Map<BookFullResponse>(b));
    }

    public async Task<BookFullResponse?> GetById(Guid id, CancellationToken cancelToken = default)
    {
        var book = await _repository.GetById(id, cancelToken);
        return _mapper.Map<BookFullResponse?>(book);
    }

    public async Task<BookFullResponse?> GetByISBN(string isbn, CancellationToken cancelToken = default)
    {
        var book = await _repository.GetByISBN(isbn, cancelToken);
        return _mapper.Map<BookFullResponse?>(book);
    }

    public async Task CreateBook(BookCreationRequest request, string coverLink, CancellationToken cancelToken = default)
    {
        var book = _mapper.Map<Book>(request);
        
        if (await _repository.Any(b => b.ISBN == book.ISBN, cancelToken))
            throw new Exception("A book with such ISBN already exists");

        Guid newId = Guid.NewGuid();
        book.CoverLink = coverLink;
        book.Id = newId;
        await _repository.Add(book, request.AuthorsId, cancelToken);
        await _bookStateService.AddBooks(newId, request.NumOfBooks, cancelToken);
    }

    public async Task EditBook(BookEditRequest request, string coverLink, CancellationToken cancelToken = default)
    {
        if (!await _repository.Any(request.Id, cancelToken))
            throw new Exception("Book not found");

        var book = _mapper.Map<Book>(request);
        var dbBook = await _repository.GetById(request.Id, cancelToken);

        if (await _repository.Any(b => b.ISBN == book.ISBN && b.Id != book.Id, cancelToken))
            throw new Exception("A book with such ISBN already exists");
        
        if (!string.IsNullOrEmpty(coverLink))
            book.CoverLink = coverLink;
        else
            book.CoverLink = dbBook!.CoverLink;
        await _repository.Update(book, request.AuthorsId, cancelToken);
    }
    public async Task DeleteBook(Guid id, CancellationToken cancelToken = default)
    {
        if (!await _repository.Any(id, cancelToken))
            throw new Exception("Book not found");
        await _repository.Delete(id, cancelToken);
    }
    public async Task<IEnumerable<BookFullResponse>> GetPage(int page, int size, FilterRequest request, CancellationToken cancelToken = default)
    {
        if (page < 1 || size < 1)
            throw new Exception("page num and size cannon be less than 1");
        var books = await _repository.GetPage(page - 1, size, (int)request.FilterType, request.Data, cancelToken);
        return books.Select(b => _mapper.Map<BookFullResponse>(b));
    }
    public async Task<IEnumerable<BookFullResponse>> GetByTitle(string title, CancellationToken cancelToken = default)
    {
        var books = await _repository.GetByTitle(title, cancelToken);
        return books.Select(b => _mapper.Map<BookFullResponse>(b));
    }
}
