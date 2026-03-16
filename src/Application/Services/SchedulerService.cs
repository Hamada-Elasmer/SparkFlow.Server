using SparkFlow.Server.Application.Abstractions.Persistence;
using SparkFlow.Server.Application.Scheduling.Selection;
using SparkFlow.Server.Domain.Entities;
using SparkFlow.Server.Domain.Enums;

namespace SparkFlow.Server.Application.Services;

public sealed class SchedulerService
{
    private readonly IAccountRepository _accounts;
    private readonly ISessionRepository _sessions;
    private readonly IWorkerRepository _workers;
    private readonly IFlowRepository _flows;
    private readonly AccountCandidateSelector _accountSelector;
    private readonly WorkerCandidateSelector _workerSelector;
    private readonly SessionService _sessionService;

    public SchedulerService(
        IAccountRepository accounts,
        ISessionRepository sessions,
        IWorkerRepository workers,
        IFlowRepository flows,
        AccountCandidateSelector accountSelector,
        WorkerCandidateSelector workerSelector,
        SessionService sessionService)
    {
        _accounts = accounts;
        _sessions = sessions;
        _workers = workers;
        _flows = flows;
        _accountSelector = accountSelector;
        _workerSelector = workerSelector;
        _sessionService = sessionService;
    }

    public async Task<Session?> ScheduleOnceAsync(CancellationToken cancellationToken = default)
    {
        var workers = await _workers.ListAsync(cancellationToken);
        var worker = _workerSelector.Select(workers);
        if (worker is null) return null;

        var accounts = await _accounts.ListAsync(cancellationToken);
        var account = _accountSelector.Select(accounts, DateTime.UtcNow);
        if (account is null) return null;

        var flow = _flows.Get("daily_run");
        if (flow is null) return null;

        var session = await _sessionService.CreateAsync(account, flow.FlowId, 1, cancellationToken);
        session.Assign(worker.Id);
        worker.AssignSession(session.Id);
        await _sessions.UpsertAsync(session, cancellationToken);
        await _workers.UpsertAsync(worker, cancellationToken);
        return session;
    }
}
