using Microsoft.EntityFrameworkCore;
using SparkFlow.Server.Application.Abstractions.Persistence;
using SparkFlow.Server.Domain.Entities;

namespace SparkFlow.Server.Infrastructure.Persistence.Repositories;

/// <summary>
/// PostgreSQL-backed repository for worker nodes.
/// This repository mirrors the behavior of the JSON repository
/// while persisting WorkerNode aggregates in PostgreSQL via EF Core.
/// </summary>
public sealed class PgWorkerRepository : IWorkerRepository
{
    private readonly SparkFlowDbContext _dbContext;

    public PgWorkerRepository(SparkFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<WorkerNode?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Workers
            .FirstOrDefaultAsync(worker => worker.Id.Value == id, cancellationToken);
    }

    public async Task<IReadOnlyList<WorkerNode>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Workers
            .OrderBy(worker => worker.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }

    public async Task UpsertAsync(WorkerNode worker, CancellationToken cancellationToken = default)
    {
        var existing = await _dbContext.Workers
            .FirstOrDefaultAsync(x => x.Id.Value == worker.Id.Value, cancellationToken);

        if (existing is null)
        {
            await _dbContext.Workers.AddAsync(worker, cancellationToken);
        }
        else
        {
            // Copy all current values from the incoming aggregate into the tracked entity.
            _dbContext.Entry(existing).CurrentValues.SetValues(worker);
        }
    }
}