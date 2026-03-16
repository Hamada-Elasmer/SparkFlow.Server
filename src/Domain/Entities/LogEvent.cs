using System;

namespace SparkFlow.Server.Domain.Entities;

/// <summary>
/// Represents a single structured log entry.
/// </summary>
public sealed class LogEvent
{
    public string Level { get; init; } = default!;
    public string Message { get; init; } = default!;
    public DateTime TimestampUtc { get; init; }
}