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

    public async Task<Guid> CreateFile(Stream stream, string fileType, CancellationToken cancelToken = default)
    {
        using (var memoryStream = new MemoryStream())
        {
            await stream.CopyToAsync(memoryStream);

            Guid guid = Guid.NewGuid();
            byte[] data = memoryStream.ToArray();
            if (await FileExists(data, cancelToken))
                return await _repository.GetIdByData(data, cancelToken);
            var file = new AppFile()
            {
                Id = guid,
                Data = data,
                Type = fileType
            };

            await _repository.Add(file, cancelToken);

            return guid;
        }
    }

    public async Task<bool> FileExists(byte[] data, CancellationToken cancelToken = default)
    {
        return await _repository.AnyByData(data, cancelToken);
    }

    public async Task Delete(Guid id, CancellationToken cancelToken)
    {
        if (!await _repository.Any(id, cancelToken))
            throw new Exception("File not found");
        await _repository.Delete(id, cancelToken);
    }

    public async Task<AppFile?> GetFileById(Guid id, CancellationToken cancelToken = default)
    {
        _cache.TryGetValue(id, out AppFile? file);
        if (file == null)
        {
            file = await _repository.GetById(id, cancelToken);
            if(file != null)
            {
                _cache.Set(id, file, TimeSpan.FromMinutes(5));
            }
        }

        if (file == null)
            throw new Exception("File not found");
        return file;
    }
}
