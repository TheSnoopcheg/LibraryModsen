using LibraryModsen.Application.Abstractions.Repositories;
using LibraryModsen.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryModsen.Persistence.Repositories;

public class AuthorsRepository(LibraryDbContext context) : BaseRepository<Author>(context), IAuthorsRepository
{
    private readonly LibraryDbContext _context = context;

    public async Task<bool> Any(Guid id, CancellationToken cancelToken = default)
    {
        return await Any(a => a.Id == id, cancelToken);
    }

    public override async Task<IEnumerable<Author>> GetAll(CancellationToken cancelToken = default)
    {
        return await _context
            .Authors
            .AsNoTracking()
            .Include(a => a.Books)
            .ToListAsync(cancelToken);
    }

    public override async Task<Author?> GetById(Guid id, CancellationToken cancelToken = default)
    {
        var author = await _context
            .Authors
            .AsNoTracking()
            .Include(a => a.Books)
            .FirstOrDefaultAsync(a => a.Id == id, cancelToken);
        if (author == null)
            throw new Exception("Author not found");
        return author;
    }

    public async Task<IEnumerable<Book>> GetAuthorBooks(Guid authorId, CancellationToken cancelToken = default)
    {
        var authorEntity = await _context
            .Authors
            .AsNoTracking()
            .Include(p => p.Books)
            .FirstOrDefaultAsync(a => a.Id == authorId, cancelToken);

        return authorEntity!.Books;
    }

    public async Task<IEnumerable<Author>> GetPage(int n, int size, CancellationToken cancelToken = default)
    {
        return await _context
            .Authors
            .AsNoTracking()
            .Skip(n * size)
            .Take(size)
            .Include(a => a.Books)
            .ToListAsync(cancelToken);
    }
}
