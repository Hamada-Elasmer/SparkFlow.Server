using SparkFlow.Server.Domain.Entities;
using SparkFlow.Server.Infrastructure.Serialization;

namespace SparkFlow.Server.Infrastructure.Persistence.Json;

public sealed class UpdateStore : JsonFileStore<UpdateManifest>
{
    public UpdateStore() : base("data/updates.json")
    {
    }
}
