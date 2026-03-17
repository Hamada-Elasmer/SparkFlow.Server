using Microsoft.EntityFrameworkCore;
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
using SparkFlow.Server.Infrastructure.Persistence;
using SparkFlow.Server.Infrastructure.Persistence.Json;
using SparkFlow.Server.Infrastructure.Persistence.Repositories;
using SparkFlow.Server.Infrastructure.Persistence.UnitOfWork;
using SparkFlow.Server.Infrastructure.Time;

namespace SparkFlow.Server.Infrastructure.DependencyInjection;

/// <summary>
/// Registers infrastructure services, repositories, and application services.
/// When a PostgreSQL connection string is available, the core persistence layer
/// uses EF Core-backed repositories. Otherwise, the application falls back to
/// the legacy JSON-based persistence implementation.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSparkFlowInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var usePostgres = !string.IsNullOrWhiteSpace(connectionString);

        // Register EF Core DbContext only when PostgreSQL is configured.
        if (usePostgres)
        {
            services.AddDbContext<SparkFlowDbContext>(options =>
                options.UseNpgsql(connectionString));
        }

        // Legacy JSON stores remain registered so the fallback mode keeps working.
        services.AddSingleton<AccountStore>();
        services.AddSingleton<FlowStore>();
        services.AddSingleton<LogStore>();
        services.AddSingleton<PolicyStore>();
        services.AddSingleton<SessionStore>();
        services.AddSingleton<UpdateStore>();
        services.AddSingleton<WorkerStore>();

        // Common infrastructure services.
        services.AddSingleton<IHasher, Sha256Hasher>();
        services.AddSingleton<ISigner, RsaSigner>();
        services.AddSingleton<IAccountExecutionLock, InProcessAccountExecutionLock>();
        services.AddSingleton<IMetricsWriter, InMemoryMetricsWriter>();
        services.AddSingleton<IClock, SystemClock>();

        // Unit of work registration.
        // Use EF Core-backed unit of work when PostgreSQL is enabled,
        // otherwise keep the legacy no-op JSON implementation.
        if (usePostgres)
        {
            services.AddScoped<IUnitOfWork, EfUnitOfWork>();
        }
        else
        {
            services.AddSingleton<IUnitOfWork, JsonUnitOfWork>();
        }

        // Repository registrations.
        if (usePostgres)
        {
            services.AddScoped<IAccountRepository, PgAccountRepository>();
            services.AddScoped<ISessionRepository, PgSessionRepository>();
            services.AddScoped<IWorkerRepository, PgWorkerRepository>();
            services.AddScoped<IFlowRepository, PgFlowRepository>();
            services.AddScoped<IPolicyRepository, PgPolicyRepository>();

            // PgLogRepository is singleton-safe because it creates an internal scope
            // for each write operation before resolving SparkFlowDbContext.
            services.AddSingleton<ILogRepository, PgLogRepository>();
        }
        else
        {
            services.AddSingleton<IAccountRepository, JsonAccountRepository>();
            services.AddSingleton<ISessionRepository, JsonSessionRepository>();
            services.AddSingleton<IWorkerRepository, JsonWorkerRepository>();
            services.AddSingleton<IFlowRepository, JsonFlowRepository>();
            services.AddSingleton<IPolicyRepository, JsonPolicyRepository>();
            services.AddSingleton<ILogRepository, JsonLogRepository>();
        }

        // Domain/application helpers.
        services.AddSingleton(new NextRunAtCalculator());
        services.AddSingleton<AccountCandidateSelector>();
        services.AddSingleton<WorkerCandidateSelector>();

        // Application services.
        // Services that depend on DbContext-backed repositories must be scoped.
        if (usePostgres)
        {
            services.AddScoped<AccountService>();
            services.AddScoped<SessionService>();
            services.AddScoped<WorkerService>();
            services.AddScoped<FlowService>();
            services.AddScoped<PolicyService>();
        }
        else
        {
            services.AddSingleton<AccountService>();
            services.AddSingleton<SessionService>();
            services.AddSingleton<WorkerService>();
            services.AddSingleton<FlowService>();
            services.AddSingleton<PolicyService>();
        }

        services.AddSingleton<MetricsService>();
        services.AddSingleton<SessionAssignmentService>();
        services.AddSingleton<SchedulerService>();
        services.AddSingleton<FailureRecoveryService>();
        services.AddSingleton<WorkerMonitorService>();

        return services;
    }
}