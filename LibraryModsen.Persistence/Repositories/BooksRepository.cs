using LibraryModsen.Application.Abstractions.Repositories;
using LibraryModsen.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryModsen.Persistence.Repositories;

public class BooksRepository(LibraryDbContext context) : IBooksRepository
{
    private readonly LibraryDbContext _context = context;

    public async Task<bool> Any(Guid id)
    {
        return await _context
            .Books
            .AsNoTracking()
            .AnyAsync(b => b.Id == id);
    }

    public async Task<IEnumerable<Book>> GetAll(int type, string fvalue)
    {
        IQueryable<Book> query = _context
            .Books
            .AsNoTracking()
            .Include(b => b.Authors);

        if (type == 1)
        {
            query = query.Where(b => b.Genre == fvalue);
        }
        else if (type == 2)
        {
            query = query.Where(b => b.Authors.Any(a => a.Id == Guid.Parse(fvalue)));
        }

        return await query.ToListAsync();
    }

    public async Task<Book?> GetById(Guid id)
    {
        return await _context
            .Books
            .AsNoTracking()
            .Include(b => b.Authors)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<Book?> GetByISBN(string iSBN)
    {
        return await _context
            .Books
            .AsNoTracking()
            .Include(b => b.Authors)
            .FirstOrDefaultAsync(b => b.ISBN == iSBN);
    }

    public async Task<IEnumerable<Book>> GetByTitle(string title)
    {
        return await _context
            .Books
            .FromSql($"select * from Books where Title like {title + "%"}")
            .AsNoTracking()
            .Include(b => b.Authors)
            .ToListAsync();
    }

    public async Task<Guid> Add(Book book, List<Guid> authorIds)
    {
        await _context
            .Books
            .AddAsync(book);
        var authors = await _context
            .Authors
            .Where(a => authorIds.Contains(a.Id))
            .ToListAsync();
        authors.ForEach(a => a.Books.Add(book));
        await _context
            .SaveChangesAsync();

        return book.Id;
    }

    public async Task<Guid> Update(Book book, List<Guid> authorsIds)
    {
        _context
            .Books
            .Update(book);
        var authorsWOBooks = await _context
            .Authors
            .Where(a => authorsIds.Contains(a.Id) && !a.Books.Any(b => b.Id == book.Id))
            .ToListAsync();
        authorsWOBooks.ForEach(a => a.Books.Add(book));
        var authorsWBooks = await _context
            .Authors
            .Include(a => a.Books)
            .Where(a => !authorsIds.Contains(a.Id) && a.Books.Any(b => b.Id == book.Id))
            .ToListAsync();
        authorsWBooks.ForEach(a => a.Books.RemoveAll(b => b.Id == book.Id));
        await _context
            .SaveChangesAsync();

        return book.Id;
    }

    public async Task<Guid> DeleteById(Guid id)
    {
        await _context
            .Books
            .Where(b => b.Id == id)
            .ExecuteDeleteAsync();

        return id;
    }

    public async Task<IEnumerable<Book>> GetPage(int n, int size, int type, string fvalue)
    {
        var query = _context
            .Books
            .AsNoTracking()
            .Include(b => b.Authors);

        if (type == 1)
        {
            query.Where(b => b.Genre == fvalue);
        }
        else if (type == 2)
        {
            query.Where(b => b.Authors.Any(a => a.Id == Guid.Parse(fvalue)));
        }

        return await query
            .Skip(n * size)
            .Take(size)
            .ToListAsync();
    }

    public async Task<IEnumerable<Book>> GetBooksByAuthor(Guid authorId)
    {
        return await _context
            .Books
            .AsNoTracking()
            .Include(p => p.Authors)
            .Where(p => p.Authors != null && p.Authors.Any(a => a.Id == authorId))
            .ToListAsync();
    }
}
