using Microsoft.EntityFrameworkCore;
using SparkFlow.Server.Application.Abstractions.Persistence;
using SparkFlow.Server.Domain.Entities;

namespace SparkFlow.Server.Infrastructure.Persistence.Repositories;

/// <summary>
/// PostgreSQL-backed repository for sessions.
/// This repository mirrors the behavior of the JSON repository
/// while persisting Session aggregates in PostgreSQL via EF Core.
/// </summary>
public sealed class PgSessionRepository : ISessionRepository
{
    private readonly SparkFlowDbContext _dbContext;

    public PgSessionRepository(SparkFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Session?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Sessions
            .FirstOrDefaultAsync(session => session.Id.Value == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Session>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Sessions
            .OrderBy(session => session.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }

    public async Task UpsertAsync(Session session, CancellationToken cancellationToken = default)
    {
        var existing = await _dbContext.Sessions
            .FirstOrDefaultAsync(x => x.Id.Value == session.Id.Value, cancellationToken);

        if (existing is null)
        {
            await _dbContext.Sessions.AddAsync(session, cancellationToken);
        }
        else
        {
            // Copy all current values from the incoming aggregate into the tracked entity.
            _dbContext.Entry(existing).CurrentValues.SetValues(session);
        }
    }
}