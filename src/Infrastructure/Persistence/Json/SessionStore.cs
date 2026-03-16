using SparkFlow.Server.Domain.Entities;
using SparkFlow.Server.Infrastructure.Serialization;

namespace SparkFlow.Server.Infrastructure.Persistence.Json;

public sealed class SessionStore : JsonFileStore<Session>
{
    public SessionStore() : base("data/sessions.json")
    {
    }
}
