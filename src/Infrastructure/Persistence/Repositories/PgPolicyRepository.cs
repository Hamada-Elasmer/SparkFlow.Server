using Microsoft.EntityFrameworkCore;
using SparkFlow.Server.Application.Abstractions.Persistence;
using SparkFlow.Server.Domain.Entities;

namespace SparkFlow.Server.Infrastructure.Persistence.Repositories;

/// <summary>
/// PostgreSQL-backed repository for policy configuration.
/// This repository preserves the same behavior as the JSON repository:
/// - GetDefaultAsync returns the first stored policy
/// - If no policy exists, a new default Policy instance is returned
/// - SaveAsync stores a single effective policy record
/// </summary>
public sealed class PgPolicyRepository : IPolicyRepository
{
    private readonly SparkFlowDbContext _dbContext;

    public PgPolicyRepository(SparkFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Policy> GetDefaultAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Policies.FirstOrDefaultAsync(cancellationToken)
               ?? new Policy();
    }

    public async Task SaveAsync(Policy policy, CancellationToken cancellationToken = default)
    {
        var existing = await _dbContext.Policies
            .FirstOrDefaultAsync(x => x.Id == policy.Id, cancellationToken);

        if (existing is null)
        {
            await _dbContext.Policies.AddAsync(policy, cancellationToken);
        }
        else
        {
            // Copy all current values from the incoming policy into the tracked entity.
            _dbContext.Entry(existing).CurrentValues.SetValues(policy);
        }
    }
}