namespace SparkFlow.Server.Domain.Events;

public sealed record SessionStarted(string SessionId, DateTime StartedAtUtc);
