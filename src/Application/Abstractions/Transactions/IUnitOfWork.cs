namespace SparkFlow.Server.Application.Abstractions.Transactions;

/// <summary>
/// Defines a unit of work boundary for persisting changes.
/// </summary>
public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}