using SparkFlow.Server.Contracts.Workers;
using SparkFlow.Server.Domain.Entities;

namespace SparkFlow.Server.Api.Mapping;

public static class WorkerMappings
{
    public static WorkerStatusDto ToDto(this WorkerNode worker) => new(worker.Id.Value, worker.Name, worker.MachineId.Value, worker.Status.ToString(), worker.CurrentSessionId, worker.LastHeartbeatAtUtc);
}
