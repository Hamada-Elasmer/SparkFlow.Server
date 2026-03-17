using SparkFlow.Server.Application.Abstractions.Persistence;
using SparkFlow.Server.Domain.Entities;
using SparkFlow.Server.Domain.Enums;
using SparkFlow.Server.Domain.ValueObjects;

namespace SparkFlow.Server.Application.Services;

/// <summary>
/// Handles account creation, retrieval, updates, and unlock operations.
/// </summary>
public sealed class AccountService
{
    private readonly IAccountRepository _repository;

    public AccountService(IAccountRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Creates a new account and persists it.
    /// </summary>
    public async Task<Account> CreateAsync(
        string gameId,
        DateTime? nextRunAtUtc,
        CancellationToken cancellationToken = default)
    {
        var account = new Account(AccountId.New(), gameId, nextRunAtUtc ?? DateTime.UtcNow);
        await _repository.UpsertAsync(account, cancellationToken);
        return account;
    }

    /// <summary>
    /// Gets a single account by its identifier.
    /// </summary>
    public Task<Account?> GetAsync(string id, CancellationToken cancellationToken = default)
        => _repository.GetByIdAsync(id, cancellationToken);

    /// <summary>
    /// Lists all accounts.
    /// </summary>
    public Task<IReadOnlyList<Account>> ListAsync(CancellationToken cancellationToken = default)
        => _repository.ListAsync(cancellationToken);

    /// <summary>
    /// Updates an existing account if found.
    /// </summary>
    public async Task<Account?> UpdateAsync(
        string id,
        string gameId,
        string status,
        DateTime nextRunAtUtc,
        CancellationToken cancellationToken = default)
    {
        var account = await _repository.GetByIdAsync(id, cancellationToken);
        if (account is null)
        {
            return null;
        }

        if (!Enum.TryParse<AccountStatus>(status, true, out var parsedStatus))
        {
            parsedStatus = AccountStatus.Active;
        }

        account.Update(gameId, parsedStatus, nextRunAtUtc);
        await _repository.UpsertAsync(account, cancellationToken);

        return account;
    }

    /// <summary>
    /// Releases the account lock if the account exists.
    /// </summary>
    public async Task UnlockAsync(string id, CancellationToken cancellationToken = default)
    {
        var account = await _repository.GetByIdAsync(id, cancellationToken);
        if (account is null)
        {
            return;
        }

        account.Unlock();
        await _repository.UpsertAsync(account, cancellationToken);
    }
}