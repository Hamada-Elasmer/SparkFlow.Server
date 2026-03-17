namespace SparkFlow.Server.Domain.Entities;

/// <summary>
/// Represents a stored flow definition.
/// </summary>
public sealed class FlowEnvelope
{
    public string FlowId { get; init; } = default!;
    public string Json { get; init; } = default!;
    public string Sha256 { get; init; } = default!;
    public string Signature { get; init; } = default!;
    public DateTime UpdatedUtc { get; init; }
}