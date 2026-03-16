using SparkFlow.Server.Domain.Entities;

namespace SparkFlow.Server.Domain.Rules;

public static class AccountPolicyRules
{
    public static bool ShouldPause(Account account, Policy policy) => policy.IsEnabled && account.FailureCount >= policy.FailureThreshold;
    public static DateTime CalculateNextRunAt(DateTime utcNow, Policy policy) => utcNow.AddMinutes(Math.Max(1, policy.CooldownMinutes));
}
