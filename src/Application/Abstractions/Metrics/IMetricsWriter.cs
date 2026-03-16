namespace SparkFlow.Server.Application.Abstractions.Metrics;

public interface IMetricsWriter
{
    void Increment(string name, double value = 1);
    void Gauge(string name, double value);
    IReadOnlyDictionary<string, double> Snapshot();
}
