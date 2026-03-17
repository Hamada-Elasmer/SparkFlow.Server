using SparkFlow.Server.Application.Abstractions.Transactions;

namespace SparkFlow.Server.Infrastructure.Persistence.UnitOfWork;

/// <summary>
/// EF Core-backed unit of work implementation.
/// This delegates transaction completion to the shared DbContext instance.
/// </summary>
public sealed class EfUnitOfWork : IUnitOfWork
{
    private readonly SparkFlowDbContext _dbContext;

    public EfUnitOfWork(SparkFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}