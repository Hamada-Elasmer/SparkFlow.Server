using SparkFlow.Server.Domain.Entities;

namespace SparkFlow.Server.Application.Abstractions.Persistence;

/// <summary>
/// Contract for persisting log batches.
/// </summary>
public interface ILogRepository
{
    Task SaveAsync(LogBatch batch);
}