using SparkFlow.Server.Domain.Entities;
using SparkFlow.Server.Infrastructure.Serialization;

namespace SparkFlow.Server.Infrastructure.Persistence.Json;

public sealed class PolicyStore : JsonFileStore<Policy>
{
    public PolicyStore() : base("data/policies.json")
    {
    }
}
