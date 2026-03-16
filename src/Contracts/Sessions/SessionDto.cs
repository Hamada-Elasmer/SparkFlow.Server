namespace SparkFlow.Server.Contracts.Sessions;

public sealed record SessionDto(
    string Id,
    string AccountId,
    string? WorkerId,
    string FlowId,
    int FlowVersion,
    string Status,
    string ResultType,
    DateTime CreatedAtUtc,
    DateTime? StartedAtUtc,
    DateTime? EndedAtUtc,
    string? Error,
    int RetryCount);
