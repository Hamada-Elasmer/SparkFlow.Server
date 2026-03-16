using SparkFlow.Server.Application.Abstractions.Persistence;
using SparkFlow.Server.Domain.Entities;
using SparkFlow.Server.Domain.ValueObjects;

namespace SparkFlow.Server.Application.Services;

public sealed class WorkerService
{
    private readonly IWorkerRepository _repository;

    public WorkerService(IWorkerRepository repository)
    {
        _repository = repository;
    }

    public async Task<WorkerNode> RegisterAsync(string name, string machineId, string version, int maxConcurrentSessions, CancellationToken cancellationToken = default)
    {
        var worker = new WorkerNode(WorkerId.New(), name, new MachineId(machineId), version, maxConcurrentSessions);
        await _repository.UpsertAsync(worker, cancellationToken);
        return worker;
    }

    public Task<WorkerNode?> GetAsync(string id, CancellationToken cancellationToken = default) => _repository.GetByIdAsync(id, cancellationToken);
    public Task<IReadOnlyList<WorkerNode>> ListAsync(CancellationToken cancellationToken = default) => _repository.ListAsync(cancellationToken);

    public async Task<WorkerNode?> HeartbeatAsync(string workerId, string? ipAddress, CancellationToken cancellationToken = default)
    {
        var worker = await _repository.GetByIdAsync(workerId, cancellationToken);
        if (worker is null) return null;
        worker.Heartbeat(DateTime.UtcNow, ipAddress);
        await _repository.UpsertAsync(worker, cancellationToken);
        return worker;
    }

    public async Task SetAssignedSessionAsync(string workerId, string sessionId, CancellationToken cancellationToken = default)
    {
        var worker = await _repository.GetByIdAsync(workerId, cancellationToken);
        if (worker is null) return;
        worker.AssignSession(new SessionId(sessionId));
        await _repository.UpsertAsync(worker, cancellationToken);
    }

    public async Task ClearAssignedSessionAsync(string workerId, CancellationToken cancellationToken = default)
    {
        var worker = await _repository.GetByIdAsync(workerId, cancellationToken);
        if (worker is null) return;
        worker.ClearSession();
        await _repository.UpsertAsync(worker, cancellationToken);
    }
}
