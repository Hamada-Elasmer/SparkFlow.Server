using SparkFlow.Server.Domain.Entities;

namespace SparkFlow.Server.Domain.Rules;

public static class WorkerRules
{
    public static bool CanAcceptWork(WorkerNode worker) => worker.IsIdle();
}
