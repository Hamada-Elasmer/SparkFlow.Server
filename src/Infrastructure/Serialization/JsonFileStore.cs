using System.Text.Json;

namespace SparkFlow.Server.Infrastructure.Serialization;

public sealed class JsonFileStore<T>
{
    private readonly string _path;
    private readonly JsonSerializerOptions _options;
    private readonly SemaphoreSlim _gate = new(1,1);

    public JsonFileStore(string path)
    {
        _path = path;
        _options = JsonSerializerOptionsFactory.Create();
        var dir = Path.GetDirectoryName(path);
        if (!string.IsNullOrWhiteSpace(dir)) Directory.CreateDirectory(dir);
    }

    public async Task<List<T>> ReadAllAsync(CancellationToken cancellationToken = default)
    {
        await _gate.WaitAsync(cancellationToken);
        try
        {
            if (!File.Exists(_path)) return new List<T>();
            await using var stream = File.OpenRead(_path);
            return await JsonSerializer.DeserializeAsync<List<T>>(stream, _options, cancellationToken) ?? new List<T>();
        }
        finally
        {
            _gate.Release();
        }
    }

    public async Task WriteAllAsync(List<T> items, CancellationToken cancellationToken = default)
    {
        await _gate.WaitAsync(cancellationToken);
        try
        {
            await using var stream = File.Create(_path);
            await JsonSerializer.SerializeAsync(stream, items, _options, cancellationToken);
        }
        finally
        {
            _gate.Release();
        }
    }
}
