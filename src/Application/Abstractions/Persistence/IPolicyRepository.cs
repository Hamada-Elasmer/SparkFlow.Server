using SparkFlow.Server.Domain.Entities;

namespace SparkFlow.Server.Application.Abstractions.Persistence;

public interface IPolicyRepository
{
    Task<Policy> GetDefaultAsync(CancellationToken cancellationToken = default);
    Task SaveAsync(Policy policy, CancellationToken cancellationToken = default);
}
