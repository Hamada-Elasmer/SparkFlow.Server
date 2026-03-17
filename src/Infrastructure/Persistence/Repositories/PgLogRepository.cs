using Microsoft.Extensions.DependencyInjection;
using SparkFlow.Server.Application.Abstractions.Persistence;
using SparkFlow.Server.Domain.Entities;

namespace SparkFlow.Server.Infrastructure.Persistence.Repositories;

/// <summary>
/// PostgreSQL-backed repository for client log batches.
/// Each LogEvent inside the incoming batch is stored as a separate row
/// in the logs table, while the batch DeviceId is stored as a shadow property.
/// </summary>
public sealed class PgLogRepository : ILogRepository
{
    private readonly IServiceScopeFactory _scopeFactory;

    public PgLogRepository(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task SaveAsync(LogBatch batch)
    {
        ArgumentNullException.ThrowIfNull(batch);

        if (batch.Events.Count == 0)
        {
            return;
        }

        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<SparkFlowDbContext>();

        foreach (var item in batch.Events)
        {
            var entry = dbContext.Logs.Add(new LogEvent
            {
                Level = item.Level,
                Message = item.Message,
                TimestampUtc = item.TimestampUtc
            });

            // Store the batch-level device identifier as a shadow property.
            entry.Property("DeviceId").CurrentValue = batch.DeviceId;
        }

        await dbContext.SaveChangesAsync();
    }
}