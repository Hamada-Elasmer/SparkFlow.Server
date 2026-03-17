using SparkFlow.Server.Domain.Entities;

namespace SparkFlow.Server.Application.Abstractions.Persistence;

/// <summary>
/// Contract for reading and persisting policy configuration.
/// </summary>
public interface IPolicyRepository
{
    Task<Policy> GetDefaultAsync(CancellationToken cancellationToken = default);

    Task SaveAsync(Policy policy, CancellationToken cancellationToken = default);
}