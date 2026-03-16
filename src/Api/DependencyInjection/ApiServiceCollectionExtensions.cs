using SparkFlow.Server.Api.BackgroundServices;
using SparkFlow.Server.Infrastructure.DependencyInjection;

namespace SparkFlow.Server.Api.DependencyInjection;

public static class ApiServiceCollectionExtensions
{
    public static IServiceCollection AddSparkFlowServer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSparkFlowInfrastructure(configuration);
        services.AddHostedService<SchedulerBackgroundService>();
        services.AddHostedService<WorkerHeartbeatMonitorBackgroundService>();
        services.AddHostedService<SessionRecoveryBackgroundService>();
        return services;
    }
}
