namespace SparkFlow.Server.Domain.ValueObjects;

public readonly record struct AccountId(string Value)
{
    public override string ToString() => Value;
    public static AccountId New() => new(Guid.NewGuid().ToString("N"));
}
