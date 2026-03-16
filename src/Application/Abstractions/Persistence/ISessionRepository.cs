using SparkFlow.Server.Domain.Entities;

namespace SparkFlow.Server.Application.Abstractions.Persistence;

public interface ISessionRepository
{
    Task<Session?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Session>> ListAsync(CancellationToken cancellationToken = default);
    Task UpsertAsync(Session session, CancellationToken cancellationToken = default);
}
