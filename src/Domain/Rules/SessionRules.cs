using SparkFlow.Server.Domain.Entities;
using SparkFlow.Server.Domain.Enums;

namespace SparkFlow.Server.Domain.Rules;

public static class SessionRules
{
    public static bool CanStart(Session session) => session.Status is SessionStatus.Created or SessionStatus.Assigned;
    public static bool IsTerminal(Session session) => session.Status is SessionStatus.Completed or SessionStatus.Failed or SessionStatus.Cancelled;
}
