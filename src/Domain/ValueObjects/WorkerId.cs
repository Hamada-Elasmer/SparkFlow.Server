namespace SparkFlow.Server.Domain.ValueObjects;

public readonly record struct WorkerId(string Value)
{
    public override string ToString() => Value;
    public static WorkerId New() => new(Guid.NewGuid().ToString("N"));
}
