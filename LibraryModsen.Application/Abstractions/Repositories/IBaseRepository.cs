using System.Linq.Expressions;

namespace LibraryModsen.Application.Abstractions.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task Add(T entity, CancellationToken cancelToken = default);
        Task<bool> Any(Expression<Func<T, bool>> filter, CancellationToken cancelToken = default);
        Task Delete(Guid id, CancellationToken cancelToken = default);
        Task<IEnumerable<T>> GetAll(CancellationToken cancelToken = default);
        Task<T?> GetById(Guid id, CancellationToken cancelToken = default);
        Task Update(T entity, CancellationToken cancelToken = default);
    }
}