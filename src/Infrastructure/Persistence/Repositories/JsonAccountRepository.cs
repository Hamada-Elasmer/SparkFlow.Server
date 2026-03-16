using SparkFlow.Server.Application.Abstractions.Persistence;
using SparkFlow.Server.Domain.Entities;
using SparkFlow.Server.Infrastructure.Persistence.Json;

namespace SparkFlow.Server.Infrastructure.Persistence.Repositories;

public sealed class JsonAccountRepository : IAccountRepository
{
    private readonly AccountStore _store;
    public JsonAccountRepository(AccountStore store) => _store = store;
    public async Task<Account?> GetByIdAsync(string id, CancellationToken cancellationToken = default) => (await _store.ReadAllAsync(cancellationToken)).FirstOrDefault(x => x.Id.Value == id);
    public async Task<IReadOnlyList<Account>> ListAsync(CancellationToken cancellationToken = default) => await _store.ReadAllAsync(cancellationToken);
    public async Task UpsertAsync(Account account, CancellationToken cancellationToken = default) { var items = await _store.ReadAllAsync(cancellationToken); var idx = items.FindIndex(x => x.Id.Value == account.Id.Value); if (idx >= 0) items[idx] = account; else items.Add(account); await _store.WriteAllAsync(items, cancellationToken); }
}
