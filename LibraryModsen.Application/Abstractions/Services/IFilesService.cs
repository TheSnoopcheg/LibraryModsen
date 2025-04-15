using LibraryModsen.Domain.Models;

namespace LibraryModsen.Application.Abstractions.Services
{
    public interface IFilesService
    {
        Task<Guid> CreateFile(Stream stream, string fileType);
        Task Delete(Guid id);
        Task<bool> FileExists(byte[] data);
        Task<AppFile?> GetFileById(Guid id);
    }
}