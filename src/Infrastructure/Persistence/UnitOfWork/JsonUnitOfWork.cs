using SparkFlow.Server.Application.Abstractions.Transactions;

namespace SparkFlow.Server.Infrastructure.Persistence.UnitOfWork;

/// <summary>
/// No-op unit of work used by the legacy JSON persistence mode.
/// JSON repositories persist immediately, so no shared commit is required.
/// </summary>
public sealed class JsonUnitOfWork : IUnitOfWork
{
    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}