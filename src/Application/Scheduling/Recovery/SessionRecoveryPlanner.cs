using SparkFlow.Server.Domain.Entities;
using SparkFlow.Server.Domain.Enums;

namespace SparkFlow.Server.Application.Scheduling.Recovery;

public sealed class SessionRecoveryPlanner
{
    public IReadOnlyList<Session> GetRecoverableSessions(IReadOnlyList<Session> sessions) =>
        sessions.Where(s => s.Status == SessionStatus.Assigned || s.Status == SessionStatus.Running).ToList();
}
