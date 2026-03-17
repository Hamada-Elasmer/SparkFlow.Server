using System.Text.Json;

namespace SparkFlow.Server.Infrastructure.Serialization;

/// <summary>
/// Generic JSON file store used by the legacy persistence layer.
/// This class must remain non-sealed because concrete stores inherit from it.
/// </summary>
public class JsonFileStore<T>
{
    private readonly string _path;
    private readonly JsonSerializerOptions _options;
    private readonly SemaphoreSlim _gate = new(1, 1);

    public JsonFileStore(string path)
    {
        _path = path;
        _options = JsonSerializerOptionsFactory.Create();

        var directory = Path.GetDirectoryName(path);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }

    /// <summary>
    /// Reads the entire file content as a list of <typeparamref name="T"/>.
    /// Returns an empty list when the file does not exist.
    /// </summary>
    public async Task<List<T>> ReadAllAsync(CancellationToken cancellationToken = default)
    {
        await _gate.WaitAsync(cancellationToken);

        try
        {
            if (!File.Exists(_path))
            {
                return new List<T>();
            }

            await using var stream = File.OpenRead(_path);

            return await JsonSerializer.DeserializeAsync<List<T>>(stream, _options, cancellationToken)
                   ?? new List<T>();
        }
        finally
        {
            _gate.Release();
        }
    }

    /// <summary>
    /// Replaces the full file content with the provided items.
    /// </summary>
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