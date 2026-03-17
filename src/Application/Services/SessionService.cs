using SparkFlow.Server.Application.Abstractions.Persistence;
using SparkFlow.Server.Application.Abstractions.Transactions;
using SparkFlow.Server.Application.Scheduling.NextRunAt;
using SparkFlow.Server.Domain.Entities;
using SparkFlow.Server.Domain.ValueObjects;

namespace SparkFlow.Server.Application.Services;

/// <summary>
/// Manages the session lifecycle and coordinates updates across
/// sessions, accounts, and workers.
/// 
/// This service uses a unit of work so related changes can be committed
/// atomically as a single persistence operation.
/// </summary>
public sealed class SessionService
{
    private readonly ISessionRepository _sessions;
    private readonly IAccountRepository _accounts;
    private readonly IWorkerRepository _workers;
    private readonly IUnitOfWork _unitOfWork;
    private readonly NextRunAtCalculator _nextRunAtCalculator;

    public SessionService(
        ISessionRepository sessions,
        IAccountRepository accounts,
        IWorkerRepository workers,
        IUnitOfWork unitOfWork,
        NextRunAtCalculator nextRunAtCalculator)
    {
        _sessions = sessions;
        _accounts = accounts;
        _workers = workers;
        _unitOfWork = unitOfWork;
        _nextRunAtCalculator = nextRunAtCalculator;
    }

    public Task<Session?> GetAsync(string id, CancellationToken cancellationToken = default)
        => _sessions.GetByIdAsync(id, cancellationToken);

    public Task<IReadOnlyList<Session>> ListAsync(CancellationToken cancellationToken = default)
        => _sessions.ListAsync(cancellationToken);

    /// <summary>
    /// Creates a new session and locks the account so it cannot be scheduled twice.
    /// Both changes are committed together through the unit of work.
    /// </summary>
    public async Task<Session> CreateAsync(
        Account account,
        string flowId,
        int flowVersion,
        CancellationToken cancellationToken = default)
    {
        var session = new Session(SessionId.New(), account.Id, flowId, flowVersion);

        account.Lock(session.Id);

        await _sessions.UpsertAsync(session, cancellationToken);
        await _accounts.UpsertAsync(account, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return session;
    }

    /// <summary>
    /// Assigns a session to a worker and marks the worker as busy.
    /// Both updates are committed as a single unit of work.
    /// </summary>
    public async Task AssignAsync(string sessionId, string workerId, CancellationToken cancellationToken = default)
    {
        var session = await _sessions.GetByIdAsync(sessionId, cancellationToken);
        var worker = await _workers.GetByIdAsync(workerId, cancellationToken);

        if (session is null || worker is null)
        {
            return;
        }

        session.Assign(worker.Id);
        worker.AssignSession(session.Id);

        await _sessions.UpsertAsync(session, cancellationToken);
        await _workers.UpsertAsync(worker, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Marks the session as running and commits the update through the unit of work.
    /// </summary>
    public async Task<bool> StartAsync(string sessionId, CancellationToken cancellationToken = default)
    {
        var session = await _sessions.GetByIdAsync(sessionId, cancellationToken);
        if (session is null)
        {
            return false;
        }

        session.Start(DateTime.UtcNow);

        await _sessions.UpsertAsync(session, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    /// <summary>
    /// Completes the session successfully, unlocks the account,
    /// and clears the assigned worker.
    /// All related updates are committed together through the unit of work.
    /// </summary>
    public async Task<bool> CompleteAsync(string sessionId, CancellationToken cancellationToken = default)
    {
        var session = await _sessions.GetByIdAsync(sessionId, cancellationToken);
        if (session is null)
        {
            return false;
        }

        var utcNow = DateTime.UtcNow;

        session.Complete(utcNow);
        await _sessions.UpsertAsync(session, cancellationToken);

        var account = await _accounts.GetByIdAsync(session.AccountId.Value, cancellationToken);
        if (account is not null)
        {
            account.ResetFailures();
            account.Unlock();
            account.MarkRun(utcNow, _nextRunAtCalculator.ForSuccess(utcNow));

            await _accounts.UpsertAsync(account, cancellationToken);
        }

        if (session.WorkerId is not null)
        {
            // WorkerId is a value object. Use its string value when querying repositories.
            var worker = await _workers.GetByIdAsync(session.WorkerId.Value.Value, cancellationToken);
            if (worker is not null)
            {
                worker.ClearSession();
                await _workers.UpsertAsync(worker, cancellationToken);
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    /// <summary>
    /// Marks the session as failed, updates account retry timing,
    /// and releases the worker.
    /// All related updates are committed together through the unit of work.
    /// </summary>
    public async Task<bool> FailAsync(string sessionId, string error, CancellationToken cancellationToken = default)
    {
        var session = await _sessions.GetByIdAsync(sessionId, cancellationToken);
        if (session is null)
        {
            return false;
        }

        var utcNow = DateTime.UtcNow;

        session.Fail(utcNow, error);
        await _sessions.UpsertAsync(session, cancellationToken);

        var account = await _accounts.GetByIdAsync(session.AccountId.Value, cancellationToken);
        if (account is not null)
        {
            account.Unlock();
            account.MarkFailure(_nextRunAtCalculator.ForFailure(utcNow));

            await _accounts.UpsertAsync(account, cancellationToken);
        }

        if (session.WorkerId is not null)
        {
            // WorkerId is a value object. Use its string value when querying repositories.
            var worker = await _workers.GetByIdAsync(session.WorkerId.Value.Value, cancellationToken);
            if (worker is not null)
            {
                worker.ClearSession();
                await _workers.UpsertAsync(worker, cancellationToken);
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}