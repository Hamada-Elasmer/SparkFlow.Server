using SparkFlow.Server.Application.Abstractions.Crypto;
using SparkFlow.Server.Application.Abstractions.Persistence;
using SparkFlow.Server.Domain.Entities;

namespace SparkFlow.Server.Infrastructure.Persistence.Repositories;

/// <summary>
/// PostgreSQL-backed repository for flow definitions.
/// This repository preserves the same public behavior as the JSON repository:
/// - Get(flowId) returns a FlowEnvelope or null
/// - Save(flowId, json) creates or updates the stored flow definition
/// </summary>
public sealed class PgFlowRepository : IFlowRepository
{
    private readonly SparkFlowDbContext _dbContext;
    private readonly IHasher _hasher;
    private readonly ISigner _signer;

    public PgFlowRepository(
        SparkFlowDbContext dbContext,
        IHasher hasher,
        ISigner signer)
    {
        _dbContext = dbContext;
        _hasher = hasher;
        _signer = signer;
    }

    public FlowEnvelope? Get(string flowId)
    {
        return _dbContext.Flows
            .FirstOrDefault(flow => flow.FlowId == flowId);
    }

    public void Save(string flowId, string json)
    {
        var existing = _dbContext.Flows
            .FirstOrDefault(flow => flow.FlowId == flowId);

        var envelope = new FlowEnvelope
        {
            FlowId = flowId,
            Json = json,
            Sha256 = _hasher.ComputeSha256(json),
            Signature = _signer.Sign(json),
            UpdatedUtc = DateTime.UtcNow
        };

        if (existing is null)
        {
            _dbContext.Flows.Add(envelope);
        }
        else
        {
            _dbContext.Entry(existing).CurrentValues.SetValues(envelope);
        }
    }
}