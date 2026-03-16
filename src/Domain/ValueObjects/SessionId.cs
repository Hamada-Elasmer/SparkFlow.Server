namespace SparkFlow.Server.Domain.ValueObjects;

public readonly record struct SessionId(string Value)
{
    public override string ToString() => Value;
    public static SessionId New() => new(Guid.NewGuid().ToString("N"));
}
