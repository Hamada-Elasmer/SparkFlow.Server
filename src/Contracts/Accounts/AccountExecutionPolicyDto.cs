namespace SparkFlow.Server.Contracts.Accounts;

public sealed record AccountExecutionPolicyDto(int MaxRunsPerDay, int CooldownMinutes, int FailureThreshold, int PauseDurationMinutes);
