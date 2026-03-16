using SparkFlow.Server.Domain.Common;

namespace SparkFlow.Server.Domain.Entities;

public sealed class Policy : AggregateRoot<string>
{
    public int MaxRunsPerDay { get; set; } = 3;
    public int CooldownMinutes { get; set; } = 60;
    public int FailureThreshold { get; set; } = 3;
    public int PauseDurationMinutes { get; set; } = 360;
    public bool IsEnabled { get; set; } = true;

    public Policy()
    {
        Id = "default";
    }
}
