namespace SparkFlow.Server.Application.Scheduling.NextRunAt;

public sealed record NextRunAtPolicy(int SuccessCooldownMinutes = 60, int FailureCooldownMinutes = 120);
