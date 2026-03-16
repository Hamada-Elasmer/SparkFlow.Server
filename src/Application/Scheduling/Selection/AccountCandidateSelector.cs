using SparkFlow.Server.Domain.Entities;

namespace SparkFlow.Server.Application.Scheduling.Selection;

public sealed class AccountCandidateSelector
{
    public Account? Select(IReadOnlyList<Account> accounts, DateTime utcNow) =>
        accounts
            .Where(a => a.IsSchedulable(utcNow))
            .OrderBy(a => a.NextRunAtUtc)
            .FirstOrDefault();
}
