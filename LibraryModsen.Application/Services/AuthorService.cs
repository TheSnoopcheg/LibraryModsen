using AutoMapper;
using LibraryModsen.Application.Abstractions.Repositories;
using LibraryModsen.Application.Abstractions.Services;
using LibraryModsen.Application.Contracts.Author;
using LibraryModsen.Application.Contracts.Book;
using LibraryModsen.Domain.Models;

namespace LibraryModsen.Application.Services;

public class AuthorService(
        IAuthorsRepository repository,
        IMapper mapper) : IAuthorService
{
    private readonly IAuthorsRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<bool> Any(Guid id, CancellationToken cancelToken = default)
    {
        return await _repository.Any(id, cancelToken);
    }

    public async Task<IEnumerable<AuthorFullResponse>> GetAll(CancellationToken cancelToken = default)
    {
        var authors = await _repository.GetAll(cancelToken);
        return authors.Select(a => _mapper.Map<AuthorFullResponse>(a));
    }

    public async Task<AuthorFullResponse?> GetById(Guid id, CancellationToken cancelToken = default)
    {
        var author = await _repository.GetById(id, cancelToken);
        return _mapper.Map<AuthorFullResponse?>(author);
    }

    public async Task<IEnumerable<AuthorFullResponse>> GetPage(int page, int size, CancellationToken cancelToken = default)
    {
        if (page < 1 || size < 1)
            throw new Exception("Page num and size cannot be less than 1");
        var authors = await _repository.GetPage(page - 1, size, cancelToken);
        return authors.Select(a => _mapper.Map<AuthorFullResponse>(a));
    }

    public async Task<IEnumerable<BookFullResponse>> GetBooks(Guid id, CancellationToken cancelToken = default)
    {
        if (!await _repository.Any(id, cancelToken))
            throw new Exception("Author not found");
        var books = await _repository.GetAuthorBooks(id, cancelToken);
        return books.Select(b => _mapper.Map<BookFullResponse>(b));
    }

    public async Task CreateAuthor(AuthorCreationRequest request, CancellationToken cancelToken = default)
    {
        var author = _mapper.Map<Author>(request);
        author.Id = Guid.NewGuid();
        await _repository.Add(author, cancelToken);
    }

    public async Task EditAuthor(AuthorEditRequest request, CancellationToken cancelToken = default)
    {
        if (!await _repository.Any(request.Id, cancelToken))
            throw new Exception("Author not found");

        var author = _mapper.Map<Author>(request);
        await _repository.Update(author, cancelToken);
    }

    public async Task DeleteAuthor(Guid id, CancellationToken cancelToken = default)
    {
        if (!await _repository.Any(id, cancelToken))
            throw new Exception("Author not found");
        await _repository.Delete(id, cancelToken);
    }
}
