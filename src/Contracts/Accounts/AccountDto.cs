namespace SparkFlow.Server.Contracts.Accounts;

public sealed record AccountDto(
    string Id,
    string GameId,
    string Status,
    DateTime NextRunAtUtc,
    DateTime? LastRunAtUtc,
    int FailureCount,
    bool Locked,
    string? LockedBySessionId);
