using SparkFlow.Server.Domain.Entities;

namespace SparkFlow.Server.Application.Abstractions.Persistence;

/// <summary>
/// Contract for reading and persisting WorkerNode aggregates.
/// </summary>
public interface IWorkerRepository
{
    Task<WorkerNode?> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<WorkerNode>> ListAsync(CancellationToken cancellationToken = default);

    Task UpsertAsync(WorkerNode worker, CancellationToken cancellationToken = default);
}