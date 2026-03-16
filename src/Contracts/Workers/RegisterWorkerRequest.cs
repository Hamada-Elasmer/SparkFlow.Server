namespace SparkFlow.Server.Contracts.Workers;

public sealed record RegisterWorkerRequest(string Name, string MachineId, string Version, int MaxConcurrentSessions = 1);
