namespace SparkFlow.Server.Domain.Events;

public sealed record SessionAssigned(string SessionId, string WorkerId);
