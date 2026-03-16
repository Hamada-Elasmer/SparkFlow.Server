namespace SparkFlow.Server.Contracts.Workers;

public sealed record WorkerHeartbeatRequest(string WorkerId, string? IpAddress = null, string? CurrentSessionId = null);
