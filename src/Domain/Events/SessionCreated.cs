namespace SparkFlow.Server.Domain.Events;

public sealed record SessionCreated(string SessionId, string AccountId);
