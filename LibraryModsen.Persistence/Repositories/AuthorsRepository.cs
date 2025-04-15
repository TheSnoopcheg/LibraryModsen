using LibraryModsen.Application.Abstractions.Repositories;
using LibraryModsen.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryModsen.Persistence.Repositories;

public class AuthorsRepository(LibraryDbContext context) : IAuthorsRepository
{
    private readonly LibraryDbContext _context = context;

    public async Task<bool> Any(Guid id)
    {
        return await _context
            .Authors
            .AsNoTracking()
            .AnyAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<Author>> GetAll()
    {
        return await _context
            .Authors
            .AsNoTracking()
            .Include(a => a.Books)
            .ToListAsync();
    }

    public async Task<Author?> GetById(Guid id)
    {
        return await _context
            .Authors
            .AsNoTracking()
            .Include(a => a.Books)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Guid> Add(Author author)
    {
        await _context
            .Authors
            .AddAsync(author);
        await _context
            .SaveChangesAsync();

        return author.Id;
    }

    public async Task<Guid> Update(Author author)
    {
        _context
            .Authors
            .Update(author);
        await _context
            .SaveChangesAsync();

        return author.Id;
    }

    public async Task<Guid> Delete(Guid id)
    {
        await _context
            .Authors
            .Where(a => a.Id == id)
            .ExecuteDeleteAsync();

        return id;
    }

    public async Task<IEnumerable<Book>> GetAuthorBooks(Guid authorId)
    {
        var authorEntity = await _context
            .Authors
            .AsNoTracking()
            .Include(p => p.Books)
            .FirstOrDefaultAsync(a => a.Id == authorId);

        return authorEntity!.Books;
    }

    public async Task<IEnumerable<Author>> GetPage(int n, int size)
    {
        return await _context
            .Authors
            .AsNoTracking()
            .Skip(n * size)
            .Take(size)
            .Include(a => a.Books)
            .ToListAsync();
    }
}
