using LibraryModsen.Application.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LibraryModsen.Persistence.Repositories;

public class BaseRepository<T>(LibraryDbContext context) : IBaseRepository<T> where T : class
{
    private LibraryDbContext _context = context;
    private DbSet<T> _dbSet = context.Set<T>();
    public virtual async Task Add(T entity, CancellationToken cancelToken = default)
    {
        await _dbSet.AddAsync(entity, cancelToken);
        await _context.SaveChangesAsync();
    }
    public async Task<bool> Any(Expression<Func<T, bool>> filter, CancellationToken cancelToken = default)
    {
        return await _dbSet.AnyAsync(filter, cancelToken);
    }
    public virtual async Task Update(T entity, CancellationToken cancelToken = default)
    {
        _dbSet
            .Update(entity);
        await _context.SaveChangesAsync();
    }
    public async Task Delete(Guid id, CancellationToken cancelToken = default)
    {
        T? entity = await GetById(id, cancelToken);
        if (entity == null) return;
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }
    public virtual async Task<IEnumerable<T>> GetAll(CancellationToken cancelToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .ToListAsync(cancelToken);
    }
    public virtual async Task<T?> GetById(Guid id, CancellationToken cancelToken = default)
    {
        return await _dbSet
            .FindAsync(id, cancelToken);
    }
}
