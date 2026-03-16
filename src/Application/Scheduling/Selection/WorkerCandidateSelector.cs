using SparkFlow.Server.Domain.Entities;

namespace SparkFlow.Server.Application.Scheduling.Selection;

public sealed class WorkerCandidateSelector
{
    public WorkerNode? Select(IReadOnlyList<WorkerNode> workers) =>
        workers
            .Where(w => w.IsIdle())
            .OrderByDescending(w => w.LastHeartbeatAtUtc)
            .FirstOrDefault();
}
