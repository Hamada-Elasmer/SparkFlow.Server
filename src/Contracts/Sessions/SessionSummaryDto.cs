namespace SparkFlow.Server.Contracts.Sessions;

public sealed record SessionSummaryDto(string Id, string Status, string AccountId, string? WorkerId);
