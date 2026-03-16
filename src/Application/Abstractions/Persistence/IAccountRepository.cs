using SparkFlow.Server.Domain.Entities;

namespace SparkFlow.Server.Application.Abstractions.Persistence;

public interface IAccountRepository
{
    Task<Account?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Account>> ListAsync(CancellationToken cancellationToken = default);
    Task UpsertAsync(Account account, CancellationToken cancellationToken = default);
}
