using SparkFlow.Server.Domain.Common;
using SparkFlow.Server.Domain.Enums;
using SparkFlow.Server.Domain.ValueObjects;

namespace SparkFlow.Server.Domain.Entities;

public sealed class WorkerNode : AggregateRoot<WorkerId>
{
    public string Name { get; private set; } = string.Empty;
    public MachineId MachineId { get; private set; }
    public string Version { get; private set; } = "1.0.0";
    public WorkerStatus Status { get; private set; } = WorkerStatus.Idle;
    public string? CurrentSessionId { get; private set; }
    public int MaxConcurrentSessions { get; private set; } = 1;
    public DateTime LastHeartbeatAtUtc { get; private set; } = DateTime.UtcNow;
    public string? LastSeenIp { get; private set; }
    public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;
    public DateTime UpdatedAtUtc { get; private set; } = DateTime.UtcNow;

    public WorkerNode() { }

    public WorkerNode(WorkerId id, string name, MachineId machineId, string version, int maxConcurrentSessions)
    {
        Id = id;
        Name = name;
        MachineId = machineId;
        Version = version;
        MaxConcurrentSessions = Math.Max(1, maxConcurrentSessions);
        Status = WorkerStatus.Idle;
        CreatedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public bool IsIdle() => Status == WorkerStatus.Idle && string.IsNullOrWhiteSpace(CurrentSessionId);

    public void Heartbeat(DateTime utcNow, string? ipAddress = null)
    {
        LastHeartbeatAtUtc = utcNow;
        LastSeenIp = ipAddress ?? LastSeenIp;
        if (Status == WorkerStatus.Offline)
        {
            Status = string.IsNullOrWhiteSpace(CurrentSessionId) ? WorkerStatus.Idle : WorkerStatus.Busy;
        }
        UpdatedAtUtc = utcNow;
    }

    public void AssignSession(SessionId sessionId)
    {
        CurrentSessionId = sessionId.Value;
        Status = WorkerStatus.Busy;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void ClearSession()
    {
        CurrentSessionId = null;
        Status = WorkerStatus.Idle;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void MarkOffline(DateTime utcNow)
    {
        Status = WorkerStatus.Offline;
        UpdatedAtUtc = utcNow;
    }
}
