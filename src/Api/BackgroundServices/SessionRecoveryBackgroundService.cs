using SparkFlow.Server.Application.Services;

namespace SparkFlow.Server.Api.BackgroundServices;

public sealed class SessionRecoveryBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public SessionRecoveryBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var recovery = scope.ServiceProvider.GetRequiredService<FailureRecoveryService>();
            await recovery.RecoverOfflineWorkersAsync(TimeSpan.FromMinutes(2), stoppingToken);
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}
