using SparkFlow.Server.Domain.Entities;
using SparkFlow.Server.Infrastructure.Serialization;

namespace SparkFlow.Server.Infrastructure.Persistence.Json;

public sealed class LogStore : JsonFileStore<LogBatch>
{
    public LogStore() : base("data/logs/batches.json")
    {
    }
}
