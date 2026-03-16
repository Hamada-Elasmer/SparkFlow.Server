namespace SparkFlow.Server.Domain.ValueObjects;

public readonly record struct FlowId(string Value)
{
    public override string ToString() => Value;
    public static FlowId New() => new(Guid.NewGuid().ToString("N"));
}
