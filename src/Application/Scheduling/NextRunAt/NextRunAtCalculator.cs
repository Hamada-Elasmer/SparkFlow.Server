namespace SparkFlow.Server.Application.Scheduling.NextRunAt;

public sealed class NextRunAtCalculator
{
    private readonly NextRunAtPolicy _policy;

    public NextRunAtCalculator(NextRunAtPolicy? policy = null)
    {
        _policy = policy ?? new NextRunAtPolicy();
    }

    public DateTime ForSuccess(DateTime utcNow) => utcNow.AddMinutes(_policy.SuccessCooldownMinutes);
    public DateTime ForFailure(DateTime utcNow) => utcNow.AddMinutes(_policy.FailureCooldownMinutes);
}
