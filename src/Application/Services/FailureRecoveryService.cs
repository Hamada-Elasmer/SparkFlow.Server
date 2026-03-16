using SparkFlow.Server.Application.Abstractions.Persistence;
using SparkFlow.Server.Domain.Enums;

namespace SparkFlow.Server.Application.Services;

public sealed class FailureRecoveryService
{
    private readonly ISessionRepository _sessions;
    private readonly IAccountRepository _accounts;
    private readonly IWorkerRepository _workers;

    public FailureRecoveryService(ISessionRepository sessions, IAccountRepository accounts, IWorkerRepository workers)
    {
        _sessions = sessions;
        _accounts = accounts;
        _workers = workers;
    }

    public async Task<int> RecoverOfflineWorkersAsync(TimeSpan timeout, CancellationToken cancellationToken = default)
    {
        var workers = await _workers.ListAsync(cancellationToken);
        var stale = workers.Where(w => DateTime.UtcNow - w.LastHeartbeatAtUtc > timeout).ToList();
        var count = 0;

        foreach (var worker in stale)
        {
            worker.MarkOffline(DateTime.UtcNow);
            await _workers.UpsertAsync(worker, cancellationToken);
            if (!string.IsNullOrWhiteSpace(worker.CurrentSessionId))
            {
                var session = await _sessions.GetByIdAsync(worker.CurrentSessionId, cancellationToken);
                if (session is not null && session.Status is SessionStatus.Assigned or SessionStatus.Running)
                {
                    session.Fail(DateTime.UtcNow, "worker_timeout");
                    await _sessions.UpsertAsync(session, cancellationToken);
                    var account = await _accounts.GetByIdAsync(session.AccountId.Value, cancellationToken);
                    if (account is not null)
                    {
                        account.Unlock();
                        await _accounts.UpsertAsync(account, cancellationToken);
                    }
                    worker.ClearSession();
                    await _workers.UpsertAsync(worker, cancellationToken);
                }
            }
            count++;
        }

        return count;
    }
}
