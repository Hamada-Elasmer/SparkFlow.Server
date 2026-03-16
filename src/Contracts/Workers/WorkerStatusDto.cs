namespace SparkFlow.Server.Contracts.Workers;

public sealed record WorkerStatusDto(string WorkerId, string Name, string MachineId, string Status, string? CurrentSessionId, DateTime LastHeartbeatAtUtc);
