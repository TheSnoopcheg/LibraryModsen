using LibraryModsen.Domain.Models;

namespace LibraryModsen.Application.Abstractions.Services
{
    public interface IFilesService
    {
        Task<Guid> CreateFile(Stream stream, string fileType, CancellationToken cancelToken = default);
        Task Delete(Guid id, CancellationToken cancelToken = default);
        Task<bool> FileExists(byte[] data, CancellationToken cancelToken = default);
        Task<AppFile?> GetFileById(Guid id, CancellationToken cancelToken = default);
    }
}