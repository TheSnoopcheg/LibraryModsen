using AutoMapper;
using LibraryModsen.Application.Abstractions.Repositories;
using LibraryModsen.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryModsen.Persistence.Repositories;

public class BooksRepository(LibraryDbContext context) : BaseRepository<Book>(context), IBooksRepository
{
    private readonly LibraryDbContext _context = context;

    public async Task<bool> Any(Guid id, CancellationToken cancelToken = default)
    {
        return await Any(b => b.Id == id, cancelToken);
    }

    public async Task<IEnumerable<Book>> GetAll(int type, string fvalue, CancellationToken cancelToken = default)
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

        return await query.ToListAsync(cancelToken);
    }

    public override async Task<Book?> GetById(Guid id, CancellationToken cancelToken = default)
    {
        var book = await _context
            .Books
            .AsNoTracking()
            .Include(b => b.Authors)
            .FirstOrDefaultAsync(b => b.Id == id, cancelToken);
        if (book == null)
            throw new Exception("Book not found");
        return book;
    }

    public async Task<Book?> GetByISBN(string iSBN, CancellationToken cancelToken = default)
    {
        var book = await _context
            .Books
            .AsNoTracking()
            .Include(b => b.Authors)
            .FirstOrDefaultAsync(b => b.ISBN == iSBN, cancelToken);
        if (book == null)
            throw new Exception("Book not found");
        return book;
    }

    public async Task<IEnumerable<Book>> GetByTitle(string title, CancellationToken cancelToken = default)
    {
        return await _context
            .Books
            .FromSql($"select * from Books where Title like {title + "%"}")
            .AsNoTracking()
            .Include(b => b.Authors)
            .ToListAsync(cancelToken);
    }

    public async Task<Guid> Add(Book book, List<Guid> authorIds, CancellationToken cancelToken = default)
    {
        await _context
            .Books
            .AddAsync(book, cancelToken);
        var test = book.Authors;
        var authors = await _context
            .Authors
            .Where(a => authorIds.Contains(a.Id))
            .ToListAsync(cancelToken);
        authors.ForEach(a => a.Books.Add(book));
        await _context
            .SaveChangesAsync(cancelToken);

        return book.Id;
    }

    public async Task<Guid> Update(Book book, List<Guid> authorsIds, CancellationToken cancelToken = default)
    {
        _context
            .Books
            .Update(book);
        var authorsWOBooks = await _context
            .Authors
            .Where(a => authorsIds.Contains(a.Id) && !a.Books.Any(b => b.Id == book.Id))
            .ToListAsync(cancelToken);
        authorsWOBooks.ForEach(a => a.Books.Add(book));
        var authorsWBooks = await _context
            .Authors
            .Include(a => a.Books)
            .Where(a => !authorsIds.Contains(a.Id) && a.Books.Any(b => b.Id == book.Id))
            .ToListAsync(cancelToken);
        authorsWBooks.ForEach(a => a.Books.RemoveAll(b => b.Id == book.Id));
        await _context
            .SaveChangesAsync(cancelToken);

        return book.Id;
    }

    public async Task<IEnumerable<Book>> GetPage(int n, int size, int type, string fvalue, CancellationToken cancelToken = default)
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
            .ToListAsync(cancelToken);
    }

    public async Task<IEnumerable<Book>> GetBooksByAuthor(Guid authorId, CancellationToken cancelToken = default)
    {
        return await _context
            .Books
            .AsNoTracking()
            .Include(p => p.Authors)
            .Where(p => p.Authors != null && p.Authors.Any(a => a.Id == authorId))
            .ToListAsync(cancelToken);
    }
}
