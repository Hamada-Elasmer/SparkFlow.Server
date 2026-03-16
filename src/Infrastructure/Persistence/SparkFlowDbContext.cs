using Microsoft.EntityFrameworkCore;

namespace SparkFlow.Server.Infrastructure.Persistence;

public sealed class SparkFlowDbContext : DbContext
{
    public SparkFlowDbContext(DbContextOptions<SparkFlowDbContext> options)
        : base(options)
    {
    }
}