namespace SparkFlow.Server.Application.Scheduling.Recovery;

public sealed record WorkerTimeoutPolicy(TimeSpan Timeout)
{
    public static WorkerTimeoutPolicy Default => new(TimeSpan.FromMinutes(2));
}
