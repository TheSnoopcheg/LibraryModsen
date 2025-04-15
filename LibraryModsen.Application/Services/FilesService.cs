using LibraryModsen.Application.Abstractions.Repositories;
using LibraryModsen.Application.Abstractions.Services;
using LibraryModsen.Domain.Models;
using Microsoft.Extensions.Caching.Memory;

namespace LibraryModsen.Application.Services;

public class FilesService(
        IFilesRepository repository,
        IMemoryCache cache) : IFilesService
{
    private readonly IFilesRepository _repository = repository;
    private readonly IMemoryCache _cache = cache;

    public async Task<Guid> CreateFile(Stream stream, string fileType)
    {
        using (var memoryStream = new MemoryStream())
        {
            await stream.CopyToAsync(memoryStream);

            Guid guid = Guid.NewGuid();
            byte[] data = memoryStream.ToArray();
            if (await FileExists(data))
                return await _repository.GetIdByData(data);
            var file = new AppFile()
            {
                Id = guid,
                Data = data,
                Type = fileType
            };

            await _repository.Add(file);

            return guid;
        }
    }

    public async Task<bool> FileExists(byte[] data)
    {
        return await _repository.AnyByData(data);
    }

    public async Task Delete(Guid id)
    {
        if (!await _repository.Any(id))
            return;
        await _repository.Delete(id);
    }

    public async Task<AppFile?> GetFileById(Guid id)
    {
        _cache.TryGetValue(id, out AppFile? file);
        if (file == null)
        {
            file = await _repository.GetById(id);
            if(file != null)
            {
                _cache.Set(id, file, TimeSpan.FromMinutes(5));
            }
        }

        return file;
    }
}
