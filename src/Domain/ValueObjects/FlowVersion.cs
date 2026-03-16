namespace SparkFlow.Server.Domain.ValueObjects;

public readonly record struct FlowVersion(int Value)
{
    public override string ToString() => Value.ToString();
}
