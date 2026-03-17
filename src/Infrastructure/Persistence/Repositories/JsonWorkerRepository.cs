using SparkFlow.Server.Application.Abstractions.Persistence;
using SparkFlow.Server.Domain.Entities;
using SparkFlow.Server.Infrastructure.Persistence.Json;

namespace SparkFlow.Server.Infrastructure.Persistence.Repositories;

/// <summary>
/// Legacy JSON-backed repository for worker nodes.
/// This remains as a fallback when PostgreSQL is not configured.
/// </summary>
public sealed class JsonWorkerRepository : IWorkerRepository
{
    private readonly WorkerStore _store;

    public JsonWorkerRepository(WorkerStore store)
    {
        _store = store;
    }

    public async Task<WorkerNode?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var items = await _store.ReadAllAsync(cancellationToken);
        return items.FirstOrDefault(x => x.Id.Value == id);
    }

    public async Task<IReadOnlyList<WorkerNode>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await _store.ReadAllAsync(cancellationToken);
    }

    public async Task UpsertAsync(WorkerNode worker, CancellationToken cancellationToken = default)
    {
        var items = await _store.ReadAllAsync(cancellationToken);

        var index = items.FindIndex(x => x.Id.Value == worker.Id.Value);
        if (index >= 0)
        {
            items[index] = worker;
        }
        else
        {
            items.Add(worker);
        }

        await _store.WriteAllAsync(items, cancellationToken);
    }
}