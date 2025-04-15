using LibraryModsen.Domain.Models;

namespace LibraryModsen.Application.Abstractions.Repositories;

public interface IFilesRepository
{
    Task Add(AppFile file);
    Task<bool> Any(Guid id);
    Task<bool> AnyByData(byte[] data);
    Task Delete(Guid id);
    Task<AppFile?> GetById(Guid id);
    Task<Guid> GetIdByData(byte[] data);
}