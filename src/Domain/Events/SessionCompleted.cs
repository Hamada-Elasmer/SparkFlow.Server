namespace SparkFlow.Server.Domain.Events;

public sealed record SessionCompleted(string SessionId, DateTime EndedAtUtc);
