using SparkFlow.Server.Application.Services;

namespace SparkFlow.Server.Api.BackgroundServices;

public sealed class SchedulerBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SchedulerBackgroundService> _logger;

    public SchedulerBackgroundService(IServiceProvider serviceProvider, ILogger<SchedulerBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var scheduler = scope.ServiceProvider.GetRequiredService<SchedulerService>();
            var session = await scheduler.ScheduleOnceAsync(stoppingToken);
            if (session is not null)
            {
                _logger.LogInformation("Scheduled session {SessionId} for account {AccountId}", session.Id.Value, session.AccountId.Value);
            }
            await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);
        }
    }
}
