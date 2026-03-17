using SparkFlow.Server.Application.Abstractions.Persistence;
using SparkFlow.Server.Domain.Entities;
using SparkFlow.Server.Infrastructure.Persistence.Json;

namespace SparkFlow.Server.Infrastructure.Persistence.Repositories;

/// <summary>
/// Legacy JSON-backed repository for logs.
/// Used when PostgreSQL is not configured.
/// </summary>
public sealed class JsonLogRepository : ILogRepository
{
    private readonly LogStore _store;

    public JsonLogRepository(LogStore store)
    {
        _store = store;
    }

    public async Task SaveAsync(LogBatch batch)
    {
        var items = await _store.ReadAllAsync();
        items.Add(batch);

        await _store.WriteAllAsync(items);
    }
}