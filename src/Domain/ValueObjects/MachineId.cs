namespace SparkFlow.Server.Domain.ValueObjects;

public readonly record struct MachineId(string Value)
{
    public override string ToString() => Value;
    public static MachineId New() => new(Guid.NewGuid().ToString("N"));
}
