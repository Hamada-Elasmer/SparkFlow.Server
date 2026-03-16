namespace SparkFlow.Server.Contracts.Workers;

public sealed record WorkerHeartbeatResponse(bool Accepted, string Status, DateTime ServerUtc);
