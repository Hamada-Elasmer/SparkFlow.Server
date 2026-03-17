using Microsoft.EntityFrameworkCore;
using SparkFlow.Server.Application.Abstractions.Persistence;
using SparkFlow.Server.Domain.Entities;

namespace SparkFlow.Server.Infrastructure.Persistence.Repositories;

/// <summary>
/// PostgreSQL-backed repository for accounts.
/// This is the first repository migrated from JSON persistence to EF Core.
/// </summary>
public sealed class PgAccountRepository : IAccountRepository
{
    private readonly SparkFlowDbContext _dbContext;

    public PgAccountRepository(SparkFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Account?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Accounts
            .FirstOrDefaultAsync(account => account.Id.Value == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Account>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Accounts
            .OrderBy(account => account.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }

    public async Task UpsertAsync(Account account, CancellationToken cancellationToken = default)
    {
        var existing = await _dbContext.Accounts
            .FirstOrDefaultAsync(x => x.Id.Value == account.Id.Value, cancellationToken);

        if (existing is null)
        {
            await _dbContext.Accounts.AddAsync(account, cancellationToken);
        }
        else
        {
            _dbContext.Entry(existing).CurrentValues.SetValues(account);
        }
    }
}