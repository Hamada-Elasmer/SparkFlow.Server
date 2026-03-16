namespace SparkFlow.Server.Contracts.Accounts;

public sealed record UpdateAccountRequest(string GameId, string Status, DateTime NextRunAtUtc);
