using SparkFlow.Server.Domain.Entities;

namespace SparkFlow.Server.Application.Abstractions.Persistence;

/// <summary>
/// Contract for reading and persisting Session aggregates.
/// </summary>
public interface ISessionRepository
{
    Task<Session?> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Session>> ListAsync(CancellationToken cancellationToken = default);

    Task UpsertAsync(Session session, CancellationToken cancellationToken = default);
}