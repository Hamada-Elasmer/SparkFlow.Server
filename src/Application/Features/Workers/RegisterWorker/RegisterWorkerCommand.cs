namespace SparkFlow.Server.Application.Features.Workers.RegisterWorker; public sealed record RegisterWorkerCommand(string Name, string MachineId, string Version, int MaxConcurrentSessions);
