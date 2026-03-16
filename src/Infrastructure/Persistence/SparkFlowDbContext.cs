using Microsoft.EntityFrameworkCore;
using SparkFlow.Server.Domain.Entities;

namespace SparkFlow.Server.Infrastructure.Persistence;

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

        // schema
        modelBuilder.HasDefaultSchema("public");

        // tables
        modelBuilder.Entity<Account>().ToTable("accounts");
        modelBuilder.Entity<FlowEnvelope>().ToTable("flows");
        modelBuilder.Entity<Session>().ToTable("sessions");
        modelBuilder.Entity<WorkerNode>().ToTable("workers");
        modelBuilder.Entity<Policy>().ToTable("policies");
        modelBuilder.Entity<LogEvent>().ToTable("logs");

        // indexes
        modelBuilder.Entity<Account>()
            .HasIndex(x => x.Id)
            .IsUnique();

        modelBuilder.Entity<Session>()
            .HasIndex(x => x.AccountId);

        modelBuilder.Entity<WorkerNode>()
            .HasIndex(x => x.Id);

        modelBuilder.Entity<FlowEnvelope>()
            .HasIndex(x => x.Id);
    }
}