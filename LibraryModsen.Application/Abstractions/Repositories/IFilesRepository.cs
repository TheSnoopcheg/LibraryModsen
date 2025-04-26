using LibraryModsen.Domain.Models;

namespace LibraryModsen.Application.Abstractions.Repositories;

public interface IFilesRepository : IBaseRepository<AppFile>
{
    Task<bool> Any(Guid id, CancellationToken cancelToken = default);
    Task<Guid> GetIdByData(byte[] data, CancellationToken cancelToken = default);
    Task<bool> AnyByData(byte[] data, CancellationToken cancelToken = default);
}