using SparkFlow.Server.Domain.Entities;
using SparkFlow.Server.Infrastructure.Serialization;

namespace SparkFlow.Server.Infrastructure.Persistence.Json;

public sealed class FlowStore : JsonFileStore<FlowEnvelope>
{
    public FlowStore() : base("data/flows/index.json")
    {
    }
}
