using SparkFlow.Server.Contracts.Sessions;
using SparkFlow.Server.Domain.Entities;

namespace SparkFlow.Server.Api.Mapping;

public static class SessionMappings
{
    public static SessionDto ToDto(this Session session) => new(
        session.Id.Value,
        session.AccountId.Value,
        session.WorkerId?.Value,
        session.FlowId,
        session.FlowVersion,
        session.Status.ToString(),
        session.ResultType.ToString(),
        session.CreatedAtUtc,
        session.StartedAtUtc,
        session.EndedAtUtc,
        session.Error,
        session.RetryCount);
}
