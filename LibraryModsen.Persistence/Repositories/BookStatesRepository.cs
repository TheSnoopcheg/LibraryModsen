using LibraryModsen.Application.Abstractions.Repositories;
using LibraryModsen.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryModsen.Persistence.Repositories;

public class BookStatesRepository(LibraryDbContext context) : IBookStatesRepository
{
    private readonly LibraryDbContext _context = context;

    public async Task<bool> Any(Guid id)
    {
        return await _context
            .BookStates
            .AsNoTracking()
            .AnyAsync(b => b.Id == id);
    }

    public async Task<bool> AnyByBook(Guid bookId)
    {
        return await _context
            .BookStates
            .AsNoTracking()
            .AnyAsync(b => b.BookId == bookId);
    }

    public async Task<int> GetNumByBook(Guid id)
    {
        return await _context
            .BookStates
            .AsNoTracking()
            .CountAsync(b => b.BookId == id);
    }

    public async Task<int> GetAvailableNumByBook(Guid id)
    {
        return await _context
            .BookStates
            .AsNoTracking()
            .CountAsync(b => !b.IsTaken && b.BookId == id);
    }

    public async Task<IEnumerable<BookState>> GetAll()
    {
        return await _context
            .BookStates
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<BookState?> GetFirstAvalaibleByBook(Guid bookId)
    {
        return await _context
            .BookStates
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.BookId == bookId && !b.IsTaken);
    }

    public async Task<IEnumerable<BookState>> GetAllByBook(Guid id)
    {
        return await _context
            .BookStates
            .AsNoTracking()
            .Where(b => b.BookId == id)
            .ToListAsync();
    }

    public async Task<BookState?> GetById(Guid id)
    {
        return await _context
            .BookStates
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<Guid> Add(BookState bookState)
    {
        await _context
            .BookStates
            .AddAsync(bookState);
        await _context
            .SaveChangesAsync();
        return bookState.Id;
    }

    public async Task AddRange(IEnumerable<BookState> bookStates)
    {
        await _context
            .BookStates
            .AddRangeAsync(bookStates);
        await _context
            .SaveChangesAsync();
    }

    public async Task<Guid> Update(BookState bookState)
    {
        _context
            .BookStates
            .Update(bookState);

        await _context
            .SaveChangesAsync();

        return bookState.Id;
    }

    public async Task<Guid> Delete(Guid id)
    {
        await _context
            .BookStates
            .Where(b => b.Id == id)
            .ExecuteDeleteAsync();
        return id;
    }
}
