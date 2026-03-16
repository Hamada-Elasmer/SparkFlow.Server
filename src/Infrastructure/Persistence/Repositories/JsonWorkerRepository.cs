using SparkFlow.Server.Application.Abstractions.Persistence;
using SparkFlow.Server.Domain.Entities;
using SparkFlow.Server.Infrastructure.Persistence.Json;

namespace SparkFlow.Server.Infrastructure.Persistence.Repositories;

public sealed class JsonWorkerRepository : IWorkerRepository
{
    private readonly WorkerStore _store;
    public JsonWorkerRepository(WorkerStore store) => _store = store;
    public async Task<WorkerNode?> GetByIdAsync(string id, CancellationToken cancellationToken = default) => (await _store.ReadAllAsync(cancellationToken)).FirstOrDefault(x => x.Id.Value == id);
    public async Task<IReadOnlyList<WorkerNode>> ListAsync(CancellationToken cancellationToken = default) => await _store.ReadAllAsync(cancellationToken);
    public async Task UpsertAsync(WorkerNode worker, CancellationToken cancellationToken = default) { var items = await _store.ReadAllAsync(cancellationToken); var idx = items.FindIndex(x => x.Id.Value == worker.Id.Value); if (idx >= 0) items[idx] = worker; else items.Add(worker); await _store.WriteAllAsync(items, cancellationToken); }
}
