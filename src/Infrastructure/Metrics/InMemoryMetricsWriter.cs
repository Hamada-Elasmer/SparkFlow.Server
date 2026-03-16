using System.Collections.Concurrent;
using SparkFlow.Server.Application.Abstractions.Metrics;

namespace SparkFlow.Server.Infrastructure.Metrics;

public sealed class InMemoryMetricsWriter : IMetricsWriter
{
    private readonly ConcurrentDictionary<string, double> _values = new();
    public void Increment(string name, double value = 1) => _values.AddOrUpdate(name, value, (_, old) => old + value);
    public void Gauge(string name, double value) => _values.AddOrUpdate(name, value, (_, _) => value);
    public IReadOnlyDictionary<string, double> Snapshot() => new Dictionary<string, double>(_values);
}
