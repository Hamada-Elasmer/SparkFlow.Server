namespace SparkFlow.Server.Domain.Events;

public sealed record SessionFailed(string SessionId, string Error, DateTime EndedAtUtc);
