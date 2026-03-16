using SparkFlow.Server.Application.Abstractions.Metrics;

namespace SparkFlow.Server.Application.Services;

public sealed class MetricsService
{
    private readonly IMetricsWriter _metrics;

    public MetricsService(IMetricsWriter metrics)
    {
        _metrics = metrics;
    }

    public void Increment(string name, double value = 1) => _metrics.Increment(name, value);
    public IReadOnlyDictionary<string, double> Snapshot() => _metrics.Snapshot();
}
