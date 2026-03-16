using SparkFlow.Server.Contracts.Accounts;
using SparkFlow.Server.Domain.Entities;

namespace SparkFlow.Server.Api.Mapping;

public static class AccountMappings
{
    public static AccountDto ToDto(this Account account) => new(
        account.Id.Value,
        account.GameId,
        account.Status.ToString(),
        account.NextRunAtUtc,
        account.LastRunAtUtc,
        account.FailureCount,
        account.Locked,
        account.LockedBySessionId);
}
