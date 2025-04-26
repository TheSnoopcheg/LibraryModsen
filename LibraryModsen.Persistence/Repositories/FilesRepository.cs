using LibraryModsen.Application.Abstractions.Repositories;
using LibraryModsen.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryModsen.Persistence.Repositories;

public class FilesRepository(LibraryDbContext context) : BaseRepository<AppFile>(context), IFilesRepository
{
    private readonly LibraryDbContext _context = context;

    public async Task<bool> Any(Guid id, CancellationToken cancelToken = default)
    {
        return await Any(f => f.Id == id, cancelToken);
    }

    public async Task<bool> AnyByData(byte[] data, CancellationToken cancelToken = default)
    {
        return await _context
            .Files
            .AnyAsync(f => f.Data == data, cancelToken);
    }
    
    public async Task<Guid> GetIdByData(byte[] data, CancellationToken cancelToken = default)
    {
        var appFile = await _context
            .Files
            .FirstOrDefaultAsync(f => f.Data == data, cancelToken);
        if(appFile == null) return Guid.Empty;
        return appFile.Id;
    }
}
