using SparkFlow.Server.Domain.Entities;
using SparkFlow.Server.Infrastructure.Serialization;

namespace SparkFlow.Server.Infrastructure.Persistence.Json;

public sealed class WorkerStore : JsonFileStore<WorkerNode>
{
    public WorkerStore() : base("data/workers.json")
    {
    }
}
