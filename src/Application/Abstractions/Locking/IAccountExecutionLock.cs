namespace SparkFlow.Server.Application.Abstractions.Locking;

public interface IAccountExecutionLock
{
    Task<bool> TryAcquireAsync(string accountId, string sessionId, CancellationToken cancellationToken = default);
    Task ReleaseAsync(string accountId, CancellationToken cancellationToken = default);
}
