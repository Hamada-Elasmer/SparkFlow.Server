using SparkFlow.Server.Application.Services;

namespace SparkFlow.Server.Api.BackgroundServices;

public sealed class WorkerHeartbeatMonitorBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public WorkerHeartbeatMonitorBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var monitor = scope.ServiceProvider.GetRequiredService<WorkerMonitorService>();
            await monitor.ScanAsync(TimeSpan.FromMinutes(2), stoppingToken);
            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }
}
