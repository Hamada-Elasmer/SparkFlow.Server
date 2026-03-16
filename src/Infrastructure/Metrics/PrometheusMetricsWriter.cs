using SparkFlow.Server.Application.Abstractions.Metrics;

namespace SparkFlow.Server.Infrastructure.Metrics;

public sealed class PrometheusMetricsWriter : IMetricsWriter
{
    private readonly InMemoryMetricsWriter _inner = new();
    public void Increment(string name, double value = 1) => _inner.Increment(name, value);
    public void Gauge(string name, double value) => _inner.Gauge(name, value);
    public IReadOnlyDictionary<string, double> Snapshot() => _inner.Snapshot();
}
