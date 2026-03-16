namespace SparkFlow.Server.Domain.Events;

public sealed record WorkerHeartbeatReceived(string WorkerId, DateTime ReceivedAtUtc);
