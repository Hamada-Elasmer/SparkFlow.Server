using System.Collections.Generic;


namespace SparkFlow.Server.Domain.Entities;

/// <summary>
/// Represents a batch of log events sent from a client.
/// </summary>
public sealed class LogBatch
{
    public string DeviceId { get; init; } = default!;
    public List<LogEvent> Events { get; init; } = new();
}