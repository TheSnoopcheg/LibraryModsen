using LibraryModsen.Application.Abstractions.Repositories;
using LibraryModsen.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryModsen.Persistence.Repositories;

public class BookStatesRepository(LibraryDbContext context) : BaseRepository<BookState>(context), IBookStatesRepository
{
    private readonly LibraryDbContext _context = context;

    public async Task<bool> Any(Guid id, CancellationToken cancelToken = default)
    {
        return await Any(b => b.Id == id, cancelToken);
    }

    public async Task<bool> AnyByBook(Guid bookId, CancellationToken cancelToken = default)
    {
        return await _context
            .BookStates
            .AsNoTracking()
            .AnyAsync(b => b.BookId == bookId, cancelToken);
    }

    public async Task<int> GetNumByBook(Guid id, CancellationToken cancelToken = default)
    {
        return await _context
            .BookStates
            .AsNoTracking()
            .CountAsync(b => b.BookId == id, cancelToken);
    }

    public async Task<int> GetAvailableNumByBook(Guid id, CancellationToken cancelToken = default)
    {
        return await _context
            .BookStates
            .AsNoTracking()
            .CountAsync(b => !b.IsTaken && b.BookId == id, cancelToken);
    }

    public async Task<BookState?> GetFirstAvalaibleByBook(Guid bookId, CancellationToken cancelToken = default)
    {
        return await _context
            .BookStates
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.BookId == bookId && !b.IsTaken, cancelToken);
    }

    public async Task<IEnumerable<BookState>> GetAllByBook(Guid id, CancellationToken cancelToken = default)
    {
        return await _context
            .BookStates
            .AsNoTracking()
            .Where(b => b.BookId == id)
            .ToListAsync(cancelToken);
    }

    public async Task AddRange(IEnumerable<BookState> bookStates, CancellationToken cancelToken = default)
    {
        await _context
            .BookStates
            .AddRangeAsync(bookStates, cancelToken);
        await _context
            .SaveChangesAsync(cancelToken);
    }
}
