using System.Collections.Concurrent;
using SparkFlow.Server.Application.Abstractions.Locking;

namespace SparkFlow.Server.Infrastructure.Locking;

public sealed class InProcessAccountExecutionLock : IAccountExecutionLock
{
    private readonly ConcurrentDictionary<string, string> _locks = new();
    public Task<bool> TryAcquireAsync(string accountId, string sessionId, CancellationToken cancellationToken = default) => Task.FromResult(_locks.TryAdd(accountId, sessionId));
    public Task ReleaseAsync(string accountId, CancellationToken cancellationToken = default) { _locks.TryRemove(accountId, out _); return Task.CompletedTask; }
}
