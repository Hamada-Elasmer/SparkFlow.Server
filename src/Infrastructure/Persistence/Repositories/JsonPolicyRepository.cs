using SparkFlow.Server.Application.Abstractions.Persistence;
using SparkFlow.Server.Domain.Entities;
using SparkFlow.Server.Infrastructure.Persistence.Json;

namespace SparkFlow.Server.Infrastructure.Persistence.Repositories;

/// <summary>
/// Legacy JSON-backed repository for policy configuration.
/// This remains as a fallback when PostgreSQL is not configured.
/// </summary>
public sealed class JsonPolicyRepository : IPolicyRepository
{
    private readonly PolicyStore _store;

    public JsonPolicyRepository(PolicyStore store)
    {
        _store = store;
    }

    public async Task<Policy> GetDefaultAsync(CancellationToken cancellationToken = default)
    {
        return (await _store.ReadAllAsync(cancellationToken)).FirstOrDefault()
               ?? new Policy();
    }

    public async Task SaveAsync(Policy policy, CancellationToken cancellationToken = default)
    {
        await _store.WriteAllAsync(new List<Policy> { policy }, cancellationToken);
    }
}