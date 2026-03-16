using SparkFlow.Server.Domain.Entities;
using SparkFlow.Server.Infrastructure.Serialization;

namespace SparkFlow.Server.Infrastructure.Persistence.Json;

public sealed class AccountStore : JsonFileStore<Account>
{
    public AccountStore() : base("data/accounts.json")
    {
    }
}
