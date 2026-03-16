using SparkFlow.Server.Application.Abstractions.Persistence;
using SparkFlow.Server.Domain.Entities;
using SparkFlow.Server.Domain.Enums;
using SparkFlow.Server.Domain.ValueObjects;

namespace SparkFlow.Server.Application.Services;

public sealed class AccountService
{
    private readonly IAccountRepository _repository;

    public AccountService(IAccountRepository repository)
    {
        _repository = repository;
    }

    public async Task<Account> CreateAsync(string gameId, DateTime? nextRunAtUtc, CancellationToken cancellationToken = default)
    {
        var account = new Account(AccountId.New(), gameId, nextRunAtUtc ?? DateTime.UtcNow);
        await _repository.UpsertAsync(account, cancellationToken);
        return account;
    }

    public Task<Account?> GetAsync(string id, CancellationToken cancellationToken = default) => _repository.GetByIdAsync(id, cancellationToken);
    public Task<IReadOnlyList<Account>> ListAsync(CancellationToken cancellationToken = default) => _repository.ListAsync(cancellationToken);

    public async Task<Account?> UpdateAsync(string id, string gameId, string status, DateTime nextRunAtUtc, CancellationToken cancellationToken = default)
    {
        var account = await _repository.GetByIdAsync(id, cancellationToken);
        if (account is null) return null;
        if (!Enum.TryParse<AccountStatus>(status, true, out var parsed)) parsed = AccountStatus.Active;
        account.Update(gameId, parsed, nextRunAtUtc);
        await _repository.UpsertAsync(account, cancellationToken);
        return account;
    }

    public async Task UnlockAsync(string id, CancellationToken cancellationToken = default)
    {
        var account = await _repository.GetByIdAsync(id, cancellationToken);
        if (account is null) return;
        account.Unlock();
        await _repository.UpsertAsync(account, cancellationToken);
    }
}
