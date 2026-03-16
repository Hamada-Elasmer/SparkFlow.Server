namespace SparkFlow.Server.Application.Services;

public sealed class WorkerMonitorService
{
    private readonly FailureRecoveryService _failureRecoveryService;

    public WorkerMonitorService(FailureRecoveryService failureRecoveryService)
    {
        _failureRecoveryService = failureRecoveryService;
    }

    public Task<int> ScanAsync(TimeSpan timeout, CancellationToken cancellationToken = default) =>
        _failureRecoveryService.RecoverOfflineWorkersAsync(timeout, cancellationToken);
}
