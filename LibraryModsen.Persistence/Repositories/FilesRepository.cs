using LibraryModsen.Application.Abstractions.Repositories;
using LibraryModsen.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryModsen.Persistence.Repositories;

public class FilesRepository(LibraryDbContext context) : IFilesRepository
{
    private readonly LibraryDbContext _context = context;

    public async Task<AppFile?> GetById(Guid id)
    {
        return await _context
            .Files
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<bool> Any(Guid id)
    {
        return await _context
            .Files
            .AsNoTracking()
            .AnyAsync(f => f.Id == id);
    }

    public async Task<bool> AnyByData(byte[] data)
    {
        return await _context
            .Files
            .AnyAsync(f => f.Data == data);
    }

    public async Task Add(AppFile file)
    {
        await _context
            .Files
            .AddAsync(file);
        await _context
            .SaveChangesAsync();
    }
    
    public async Task<Guid> GetIdByData(byte[] data)
    {
        var appFile = await _context
            .Files
            .FirstOrDefaultAsync(f => f.Data == data);
        if(appFile == null) return Guid.Empty;
        return appFile.Id;
    }

    public async Task Delete(Guid id)
    {
        await _context
            .Files
            .Where(f => f.Id == id)
            .ExecuteDeleteAsync();
    }
}
