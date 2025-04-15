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

    public async Task<bool> Any(Guid id)
    {
        return await _repository.Any(id);
    }

    public async Task<IEnumerable<AuthorFullResponse>> GetAll()
    {
        var authors = await _repository.GetAll();
        return authors.Select(a => _mapper.Map<AuthorFullResponse>(a));
    }

    public async Task<AuthorFullResponse?> GetById(Guid id)
    {
        var author = await _repository.GetById(id);
        return _mapper.Map<AuthorFullResponse?>(author);
    }

    public async Task<IEnumerable<AuthorFullResponse>> GetPage(int page, int size)
    {
        var authors = await _repository.GetPage(page, size);
        return authors.Select(a => _mapper.Map<AuthorFullResponse>(a));
    }

    public async Task<IEnumerable<BookFullResponse>> GetBooks(Guid id)
    {
        var books = await _repository.GetAuthorBooks(id);
        return books.Select(b => _mapper.Map<BookFullResponse>(b));
    }

    public async Task<Guid> CreateAuthor(AuthorCreationRequest request)
    {
        var author = _mapper.Map<Author>(request);
        author.Id = Guid.NewGuid();
        var id = await _repository.Add(author);
        return id;
    }

    public async Task<Guid> EditAuthor(AuthorEditRequest request)
    {
        if (!await _repository.Any(request.Id))
            return Guid.Empty;

        var author = _mapper.Map<Author>(request);
        var id = await _repository.Update(author);
        return id;
    }

    public async Task<Guid> DeleteAuthor(Guid id)
    {
        if (!await _repository.Any(id))
            return Guid.Empty;
        return await _repository.Delete(id);
    }
}
