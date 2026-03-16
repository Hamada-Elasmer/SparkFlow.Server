namespace SparkFlow.Server.Contracts.Accounts;

public sealed record CreateAccountRequest(string GameId, DateTime? NextRunAtUtc = null);
