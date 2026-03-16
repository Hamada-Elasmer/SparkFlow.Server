using SparkFlow.Server.Domain.Common;
using SparkFlow.Server.Domain.Enums;
using SparkFlow.Server.Domain.ValueObjects;

namespace SparkFlow.Server.Domain.Entities;

public sealed class Account : AggregateRoot<AccountId>
{
    public string GameId { get; private set; } = string.Empty;
    public AccountStatus Status { get; private set; } = AccountStatus.Active;
    public DateTime NextRunAtUtc { get; private set; } = DateTime.UtcNow;
    public DateTime? LastRunAtUtc { get; private set; }
    public int FailureCount { get; private set; }
    public bool Locked { get; private set; }
    public string? LockedBySessionId { get; private set; }
    public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;
    public DateTime UpdatedAtUtc { get; private set; } = DateTime.UtcNow;

    public Account() { }

    public Account(AccountId id, string gameId, DateTime nextRunAtUtc)
    {
        Id = id;
        GameId = gameId;
        NextRunAtUtc = nextRunAtUtc;
        CreatedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public bool IsSchedulable(DateTime nowUtc) => Status == AccountStatus.Active && !Locked && NextRunAtUtc <= nowUtc;

    public void Lock(SessionId sessionId)
    {
        Locked = true;
        LockedBySessionId = sessionId.Value;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void Unlock()
    {
        Locked = false;
        LockedBySessionId = null;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void MarkRun(DateTime startedAtUtc, DateTime nextRunAtUtc)
    {
        LastRunAtUtc = startedAtUtc;
        NextRunAtUtc = nextRunAtUtc;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void MarkFailure(DateTime nextRunAtUtc)
    {
        FailureCount++;
        NextRunAtUtc = nextRunAtUtc;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void ResetFailures()
    {
        FailureCount = 0;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void Update(string gameId, AccountStatus status, DateTime nextRunAtUtc)
    {
        GameId = gameId;
        Status = status;
        NextRunAtUtc = nextRunAtUtc;
        UpdatedAtUtc = DateTime.UtcNow;
    }
}
