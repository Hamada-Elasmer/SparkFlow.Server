using SparkFlow.Server.Domain.Entities;

namespace SparkFlow.Server.Application.Abstractions.Persistence;

/// <summary>
/// Abstraction for flow persistence.
/// </summary>
public interface IFlowRepository
{
    FlowEnvelope? Get(string flowId);
    void Save(string flowId, string json);
}