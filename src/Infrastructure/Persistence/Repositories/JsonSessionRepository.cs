using SparkFlow.Server.Application.Abstractions.Persistence;
using SparkFlow.Server.Domain.Entities;
using SparkFlow.Server.Infrastructure.Persistence.Json;

namespace SparkFlow.Server.Infrastructure.Persistence.Repositories;

/// <summary>
/// Legacy JSON-backed repository for sessions.
/// This remains as a fallback when PostgreSQL is not configured.
/// </summary>
public sealed class JsonSessionRepository : ISessionRepository
{
    private readonly SessionStore _store;

    public JsonSessionRepository(SessionStore store)
    {
        _store = store;
    }

    public async Task<Session?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var items = await _store.ReadAllAsync(cancellationToken);
        return items.FirstOrDefault(x => x.Id.Value == id);
    }

    public async Task<IReadOnlyList<Session>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await _store.ReadAllAsync(cancellationToken);
    }

    public async Task UpsertAsync(Session session, CancellationToken cancellationToken = default)
    {
        var items = await _store.ReadAllAsync(cancellationToken);

        var index = items.FindIndex(x => x.Id.Value == session.Id.Value);
        if (index >= 0)
        {
            items[index] = session;
        }
        else
        {
            items.Add(session);
        }

        await _store.WriteAllAsync(items, cancellationToken);
    }
}