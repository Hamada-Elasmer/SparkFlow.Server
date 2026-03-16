using SparkFlow.Server.Application.Abstractions.Persistence;
using SparkFlow.Server.Application.Scheduling.NextRunAt;
using SparkFlow.Server.Domain.Entities;
using SparkFlow.Server.Domain.ValueObjects;

namespace SparkFlow.Server.Application.Services;

public sealed class SessionService
{
    private readonly ISessionRepository _sessions;
    private readonly IAccountRepository _accounts;
    private readonly IWorkerRepository _workers;
    private readonly NextRunAtCalculator _nextRunAtCalculator;

    public SessionService(ISessionRepository sessions, IAccountRepository accounts, IWorkerRepository workers, NextRunAtCalculator nextRunAtCalculator)
    {
        _sessions = sessions;
        _accounts = accounts;
        _workers = workers;
        _nextRunAtCalculator = nextRunAtCalculator;
    }

    public Task<Session?> GetAsync(string id, CancellationToken cancellationToken = default) => _sessions.GetByIdAsync(id, cancellationToken);
    public Task<IReadOnlyList<Session>> ListAsync(CancellationToken cancellationToken = default) => _sessions.ListAsync(cancellationToken);

    public async Task<Session> CreateAsync(Account account, string flowId, int flowVersion, CancellationToken cancellationToken = default)
    {
        var session = new Session(SessionId.New(), account.Id, flowId, flowVersion);
        account.Lock(session.Id);
        await _sessions.UpsertAsync(session, cancellationToken);
        await _accounts.UpsertAsync(account, cancellationToken);
        return session;
    }

    public async Task AssignAsync(string sessionId, string workerId, CancellationToken cancellationToken = default)
    {
        var session = await _sessions.GetByIdAsync(sessionId, cancellationToken);
        var worker = await _workers.GetByIdAsync(workerId, cancellationToken);
        if (session is null || worker is null) return;
        session.Assign(worker.Id);
        worker.AssignSession(session.Id);
        await _sessions.UpsertAsync(session, cancellationToken);
        await _workers.UpsertAsync(worker, cancellationToken);
    }

    public async Task<bool> StartAsync(string sessionId, CancellationToken cancellationToken = default)
    {
        var session = await _sessions.GetByIdAsync(sessionId, cancellationToken);
        if (session is null) return false;
        session.Start(DateTime.UtcNow);
        await _sessions.UpsertAsync(session, cancellationToken);
        return true;
    }

    public async Task<bool> CompleteAsync(string sessionId, CancellationToken cancellationToken = default)
    {
        var session = await _sessions.GetByIdAsync(sessionId, cancellationToken);
        if (session is null) return false;
        session.Complete(DateTime.UtcNow);
        await _sessions.UpsertAsync(session, cancellationToken);

        var account = await _accounts.GetByIdAsync(session.AccountId.Value, cancellationToken);
        if (account is not null)
        {
            account.ResetFailures();
            account.Unlock();
            account.MarkRun(DateTime.UtcNow, _nextRunAtCalculator.ForSuccess(DateTime.UtcNow));
            await _accounts.UpsertAsync(account, cancellationToken);
        }

        if (session.WorkerId is not null)
        {
            var worker = await _workers.GetByIdAsync(session.WorkerId.Value, cancellationToken);
            if (worker is not null)
            {
                worker.ClearSession();
                await _workers.UpsertAsync(worker, cancellationToken);
            }
        }
        return true;
    }

    public async Task<bool> FailAsync(string sessionId, string error, CancellationToken cancellationToken = default)
    {
        var session = await _sessions.GetByIdAsync(sessionId, cancellationToken);
        if (session is null) return false;
        session.Fail(DateTime.UtcNow, error);
        await _sessions.UpsertAsync(session, cancellationToken);

        var account = await _accounts.GetByIdAsync(session.AccountId.Value, cancellationToken);
        if (account is not null)
        {
            account.Unlock();
            account.MarkFailure(_nextRunAtCalculator.ForFailure(DateTime.UtcNow));
            await _accounts.UpsertAsync(account, cancellationToken);
        }

        if (session.WorkerId is not null)
        {
            var worker = await _workers.GetByIdAsync(session.WorkerId.Value, cancellationToken);
            if (worker is not null)
            {
                worker.ClearSession();
                await _workers.UpsertAsync(worker, cancellationToken);
            }
        }
        return true;
    }
}
