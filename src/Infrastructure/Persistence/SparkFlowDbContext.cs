using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SparkFlow.Server.Domain.Entities;
using SparkFlow.Server.Domain.ValueObjects;

namespace SparkFlow.Server.Infrastructure.Persistence;

/// <summary>
/// EF Core database context for SparkFlow.
/// Accounts and Sessions are fully mapped for PostgreSQL.
/// The remaining entities are registered with safe minimal mappings
/// so migrations and startup can succeed while other repositories
/// are still using the legacy JSON persistence layer.
/// </summary>
public sealed class SparkFlowDbContext : DbContext
{
    public SparkFlowDbContext(DbContextOptions<SparkFlowDbContext> options)
        : base(options)
    {
    }

    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<FlowEnvelope> Flows => Set<FlowEnvelope>();
    public DbSet<Session> Sessions => Set<Session>();
    public DbSet<WorkerNode> Workers => Set<WorkerNode>();
    public DbSet<Policy> Policies => Set<Policy>();
    public DbSet<LogEvent> Logs => Set<LogEvent>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("public");

        ConfigureAccounts(modelBuilder);
        ConfigureFlows(modelBuilder);
        ConfigureSessions(modelBuilder);
        ConfigureWorkers(modelBuilder);
        ConfigurePolicies(modelBuilder);
        ConfigureLogs(modelBuilder);
    }

    private static void ConfigureAccounts(ModelBuilder modelBuilder)
    {
        var accountIdConverter = new ValueConverter<AccountId, string>(
            value => value.Value,
            value => new AccountId(value));

        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("accounts");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .HasConversion(accountIdConverter)
                .HasColumnName("id");

            entity.Property(x => x.GameId)
                .HasColumnName("game_id")
                .IsRequired();

            entity.Property(x => x.Status)
                .HasConversion<string>()
                .HasColumnName("status")
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(x => x.NextRunAtUtc)
                .HasColumnName("next_run_at_utc");

            entity.Property(x => x.LastRunAtUtc)
                .HasColumnName("last_run_at_utc");

            entity.Property(x => x.FailureCount)
                .HasColumnName("failure_count");

            entity.Property(x => x.Locked)
                .HasColumnName("locked");

            entity.Property(x => x.LockedBySessionId)
                .HasColumnName("locked_by_session_id");

            entity.Property(x => x.CreatedAtUtc)
                .HasColumnName("created_at_utc");

            entity.Property(x => x.UpdatedAtUtc)
                .HasColumnName("updated_at_utc");

            entity.HasIndex(x => x.GameId);
            entity.HasIndex(x => x.Status);
            entity.HasIndex(x => x.NextRunAtUtc);
        });
    }

    private static void ConfigureFlows(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FlowEnvelope>(entity =>
        {
            entity.ToTable("flows");

            entity.HasKey(x => x.FlowId);

            entity.Property(x => x.FlowId)
                .HasColumnName("flow_id");

            entity.Property(x => x.Json)
                .HasColumnName("json")
                .IsRequired();

            entity.Property(x => x.Sha256)
                .HasColumnName("sha256")
                .IsRequired();

            entity.Property(x => x.Signature)
                .HasColumnName("signature")
                .IsRequired();

            entity.Property(x => x.UpdatedUtc)
                .HasColumnName("updated_utc");
        });
    }

    private static void ConfigureSessions(ModelBuilder modelBuilder)
    {
        var sessionIdConverter = new ValueConverter<SessionId, string>(
            value => value.Value,
            value => new SessionId(value));

        var accountIdConverter = new ValueConverter<AccountId, string>(
            value => value.Value,
            value => new AccountId(value));

        var workerIdConverter = new ValueConverter<WorkerId?, string?>(
            value => value.HasValue ? value.Value.Value : null,
            value => string.IsNullOrWhiteSpace(value) ? null : new WorkerId(value));

        modelBuilder.Entity<Session>(entity =>
        {
            entity.ToTable("sessions");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .HasConversion(sessionIdConverter)
                .HasColumnName("id");

            entity.Property(x => x.AccountId)
                .HasConversion(accountIdConverter)
                .HasColumnName("account_id")
                .IsRequired();

            entity.Property(x => x.WorkerId)
                .HasConversion(workerIdConverter)
                .HasColumnName("worker_id");

            entity.Property(x => x.FlowId)
                .HasColumnName("flow_id")
                .IsRequired();

            entity.Property(x => x.FlowVersion)
                .HasColumnName("flow_version");

            entity.Property(x => x.Status)
                .HasConversion<string>()
                .HasColumnName("status")
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(x => x.ResultType)
                .HasConversion<string>()
                .HasColumnName("result_type")
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(x => x.CreatedAtUtc)
                .HasColumnName("created_at_utc");

            entity.Property(x => x.AssignedAtUtc)
                .HasColumnName("assigned_at_utc");

            entity.Property(x => x.StartedAtUtc)
                .HasColumnName("started_at_utc");

            entity.Property(x => x.EndedAtUtc)
                .HasColumnName("ended_at_utc");

            entity.Property(x => x.Error)
                .HasColumnName("error");

            entity.Property(x => x.RetryCount)
                .HasColumnName("retry_count");

            entity.HasIndex(x => x.AccountId);
            entity.HasIndex(x => x.Status);
            entity.HasIndex(x => x.CreatedAtUtc);
        });
    }

    private static void ConfigureWorkers(ModelBuilder modelBuilder)
    {
        var workerIdConverter = new ValueConverter<WorkerId, string>(
            value => value.Value,
            value => new WorkerId(value));

        var machineIdConverter = new ValueConverter<MachineId, string>(
            value => value.Value,
            value => new MachineId(value));

        modelBuilder.Entity<WorkerNode>(entity =>
        {
            entity.ToTable("workers");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .HasConversion(workerIdConverter)
                .HasColumnName("id");

            entity.Property(x => x.Name)
                .HasColumnName("name")
                .IsRequired();

            entity.Property(x => x.MachineId)
                .HasConversion(machineIdConverter)
                .HasColumnName("machine_id")
                .IsRequired();

            entity.Property(x => x.Version)
                .HasColumnName("version");

            entity.Property(x => x.Status)
                .HasConversion<string>()
                .HasColumnName("status");

            entity.Property(x => x.CurrentSessionId)
                .HasColumnName("current_session_id");

            entity.Property(x => x.MaxConcurrentSessions)
                .HasColumnName("max_concurrent_sessions");

            entity.Property(x => x.LastHeartbeatAtUtc)
                .HasColumnName("last_heartbeat_at_utc");

            entity.Property(x => x.LastSeenIp)
                .HasColumnName("last_seen_ip");

            entity.Property(x => x.CreatedAtUtc)
                .HasColumnName("created_at_utc");

            entity.Property(x => x.UpdatedAtUtc)
                .HasColumnName("updated_at_utc");
        });
    }

    private static void ConfigurePolicies(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Policy>(entity =>
        {
            entity.ToTable("policies");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .HasColumnName("id");

            entity.Property(x => x.MaxRunsPerDay)
                .HasColumnName("max_runs_per_day");

            entity.Property(x => x.CooldownMinutes)
                .HasColumnName("cooldown_minutes");

            entity.Property(x => x.FailureThreshold)
                .HasColumnName("failure_threshold");

            entity.Property(x => x.PauseDurationMinutes)
                .HasColumnName("pause_duration_minutes");

            entity.Property(x => x.IsEnabled)
                .HasColumnName("is_enabled");
        });
    }

    private static void ConfigureLogs(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LogEvent>(entity =>
        {
            entity.ToTable("logs");

            // LogEvent does not expose an Id property in the domain model,
            // so EF uses a shadow primary key for persistence.
            entity.Property<int>("id");
            entity.HasKey("id");

            // DeviceId belongs to the batch rather than the individual log event,
            // so it is stored as a shadow property on the row.
            entity.Property<string>("DeviceId")
                .HasColumnName("device_id")
                .IsRequired();

            entity.Property(x => x.Level)
                .HasColumnName("level")
                .IsRequired();

            entity.Property(x => x.Message)
                .HasColumnName("message")
                .IsRequired();

            entity.Property(x => x.TimestampUtc)
                .HasColumnName("timestamp_utc");

            entity.HasIndex("DeviceId");
            entity.HasIndex(x => x.TimestampUtc);
        });
    }
}