using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SparkFlow.Server.Infrastructure.Persistence;

/// <summary>
/// Design-time factory used by EF Core tools.
/// This allows "dotnet ef" to create the DbContext without relying on the full app startup.
/// </summary>
public sealed class SparkFlowDbContextFactory : IDesignTimeDbContextFactory<SparkFlowDbContext>
{
    public SparkFlowDbContext CreateDbContext(string[] args)
    {
        var basePath = Directory.GetCurrentDirectory();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Connection string 'DefaultConnection' was not found. " +
                "Set ConnectionStrings__DefaultConnection or add it to appsettings.Development.json.");
        }

        var optionsBuilder = new DbContextOptionsBuilder<SparkFlowDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new SparkFlowDbContext(optionsBuilder.Options);
    }
}