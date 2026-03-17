using SparkFlow.Server.Domain.Common;
using SparkFlow.Server.Domain.Enums;
using SparkFlow.Server.Domain.ValueObjects;

namespace SparkFlow.Server.Domain.Entities;

public sealed class Session : AggregateRoot<SessionId>
{
    public AccountId AccountId { get; private set; }
    public WorkerId? WorkerId { get; private set; }
    public string FlowId { get; private set; } = string.Empty;
    public int FlowVersion { get; private set; }
    public SessionStatus Status { get; private set; } = SessionStatus.Created;
    public SessionResultType ResultType { get; private set; } = SessionResultType.Unknown;
    public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;
    public DateTime? AssignedAtUtc { get; private set; }
    public DateTime? StartedAtUtc { get; private set; }
    public DateTime? EndedAtUtc { get; private set; }
    public string? Error { get; private set; }
    public int RetryCount { get; private set; }

    public Session()
    {
    }

    public Session(SessionId id, AccountId accountId, string flowId, int flowVersion)
    {
        Id = id;
        AccountId = accountId;
        FlowId = flowId;
        FlowVersion = flowVersion;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public void Assign(WorkerId workerId)
    {
        WorkerId = workerId;
        Status = SessionStatus.Assigned;
        AssignedAtUtc = DateTime.UtcNow;
    }

    public void Start(DateTime utcNow)
    {
        Status = SessionStatus.Running;
        StartedAtUtc = utcNow;
        Error = null;
    }

    public void Complete(DateTime utcNow)
    {
        Status = SessionStatus.Completed;
        ResultType = SessionResultType.Success;
        EndedAtUtc = utcNow;
        Error = null;
    }

    public void Fail(DateTime utcNow, string error)
    {
        Status = SessionStatus.Failed;
        ResultType = SessionResultType.Failure;
        EndedAtUtc = utcNow;
        Error = error;
        RetryCount++;
    }

    public void Cancel(DateTime utcNow, string? error = null)
    {
        Status = SessionStatus.Cancelled;
        ResultType = SessionResultType.Cancelled;
        EndedAtUtc = utcNow;
        Error = error;
    }
}