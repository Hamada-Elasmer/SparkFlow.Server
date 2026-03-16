namespace SparkFlow.Server.Application.Abstractions.Transactions;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
