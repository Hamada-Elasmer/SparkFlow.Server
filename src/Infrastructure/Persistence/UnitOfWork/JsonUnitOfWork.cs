using SparkFlow.Server.Application.Abstractions.Transactions;

namespace SparkFlow.Server.Infrastructure.Persistence.UnitOfWork;

public sealed class JsonUnitOfWork : IUnitOfWork
{
    public Task SaveChangesAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
}
