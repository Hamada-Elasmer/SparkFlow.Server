using SparkFlow.Server.Application.Abstractions.Time;

namespace SparkFlow.Server.Infrastructure.Time;

public sealed class SystemClock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}
