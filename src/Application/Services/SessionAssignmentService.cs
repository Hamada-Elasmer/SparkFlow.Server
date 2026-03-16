using SparkFlow.Server.Application.Abstractions.Persistence;
using SparkFlow.Server.Application.Scheduling.Selection;
using SparkFlow.Server.Domain.Entities;
using SparkFlow.Server.Domain.Enums;

namespace SparkFlow.Server.Application.Services;

public sealed class SessionAssignmentService
{
    private readonly IWorkerRepository _workers;
    private readonly ISessionRepository _sessions;
    private readonly WorkerCandidateSelector _selector;

    public SessionAssignmentService(IWorkerRepository workers, ISessionRepository sessions, WorkerCandidateSelector selector)
    {
        _workers = workers;
        _sessions = sessions;
        _selector = selector;
    }

    public async Task<WorkerNode?> TryAssignNextCreatedSessionAsync(CancellationToken cancellationToken = default)
    {
        var sessions = await _sessions.ListAsync(cancellationToken);
        var nextSession = sessions.Where(s => s.Status == SessionStatus.Created).OrderBy(s => s.CreatedAtUtc).FirstOrDefault();
        if (nextSession is null) return null;

        var workers = await _workers.ListAsync(cancellationToken);
        var worker = _selector.Select(workers);
        if (worker is null) return null;

        nextSession.Assign(worker.Id);
        worker.AssignSession(nextSession.Id);
        await _sessions.UpsertAsync(nextSession, cancellationToken);
        await _workers.UpsertAsync(worker, cancellationToken);
        return worker;
    }
}
