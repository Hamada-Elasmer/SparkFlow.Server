using SparkFlow.Server.Application.Abstractions.Persistence;
using SparkFlow.Server.Domain.Entities;
using SparkFlow.Server.Infrastructure.Persistence.Json;

namespace SparkFlow.Server.Infrastructure.Persistence.Repositories;

public sealed class JsonSessionRepository : ISessionRepository
{
    private readonly SessionStore _store;
    public JsonSessionRepository(SessionStore store) => _store = store;
    public async Task<Session?> GetByIdAsync(string id, CancellationToken cancellationToken = default) => (await _store.ReadAllAsync(cancellationToken)).FirstOrDefault(x => x.Id.Value == id);
    public async Task<IReadOnlyList<Session>> ListAsync(CancellationToken cancellationToken = default) => await _store.ReadAllAsync(cancellationToken);
    public async Task UpsertAsync(Session session, CancellationToken cancellationToken = default) { var items = await _store.ReadAllAsync(cancellationToken); var idx = items.FindIndex(x => x.Id.Value == session.Id.Value); if (idx >= 0) items[idx] = session; else items.Add(session); await _store.WriteAllAsync(items, cancellationToken); }
}
