using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SparkFlow.Server.Application.Abstractions.Crypto;
using SparkFlow.Server.Application.Abstractions.Locking;
using SparkFlow.Server.Application.Abstractions.Metrics;
using SparkFlow.Server.Application.Abstractions.Persistence;
using SparkFlow.Server.Application.Abstractions.Time;
using SparkFlow.Server.Application.Abstractions.Transactions;
using SparkFlow.Server.Application.Scheduling.NextRunAt;
using SparkFlow.Server.Application.Scheduling.Selection;
using SparkFlow.Server.Application.Services;
using SparkFlow.Server.Infrastructure.Crypto;
using SparkFlow.Server.Infrastructure.Locking;
using SparkFlow.Server.Infrastructure.Metrics;
using SparkFlow.Server.Infrastructure.Persistence.Json;
using SparkFlow.Server.Infrastructure.Persistence.Repositories;
using SparkFlow.Server.Infrastructure.Persistence.UnitOfWork;
using SparkFlow.Server.Infrastructure.Time;

namespace SparkFlow.Server.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSparkFlowInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<AccountStore>();
        services.AddSingleton<FlowStore>();
        services.AddSingleton<LogStore>();
        services.AddSingleton<PolicyStore>();
        services.AddSingleton<SessionStore>();
        services.AddSingleton<UpdateStore>();
        services.AddSingleton<WorkerStore>();

        services.AddSingleton<IHasher, Sha256Hasher>();
        services.AddSingleton<ISigner, RsaSigner>();
        services.AddSingleton<IAccountExecutionLock, InProcessAccountExecutionLock>();
        services.AddSingleton<IMetricsWriter, InMemoryMetricsWriter>();
        services.AddSingleton<IClock, SystemClock>();
        services.AddSingleton<IUnitOfWork, JsonUnitOfWork>();

        services.AddSingleton<IAccountRepository, JsonAccountRepository>();
        services.AddSingleton<IFlowRepository, JsonFlowRepository>();
        services.AddSingleton<ILogRepository, JsonLogRepository>();
        services.AddSingleton<IPolicyRepository, JsonPolicyRepository>();
        services.AddSingleton<ISessionRepository, JsonSessionRepository>();
        services.AddSingleton<IWorkerRepository, JsonWorkerRepository>();

        services.AddSingleton(new NextRunAtCalculator());
        services.AddSingleton<AccountCandidateSelector>();
        services.AddSingleton<WorkerCandidateSelector>();
        services.AddSingleton<AccountService>();
        services.AddSingleton<FlowService>();
        services.AddSingleton<WorkerService>();
        services.AddSingleton<SessionService>();
        services.AddSingleton<PolicyService>();
        services.AddSingleton<MetricsService>();
        services.AddSingleton<SessionAssignmentService>();
        services.AddSingleton<SchedulerService>();
        services.AddSingleton<FailureRecoveryService>();
        services.AddSingleton<WorkerMonitorService>();

        return services;
    }
}
