using SparkFlow.Server.Domain.Entities;

namespace SparkFlow.Server.Application.Abstractions.Persistence;

public interface IWorkerRepository
{
    Task<WorkerNode?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<WorkerNode>> ListAsync(CancellationToken cancellationToken = default);
    Task UpsertAsync(WorkerNode worker, CancellationToken cancellationToken = default);
}
