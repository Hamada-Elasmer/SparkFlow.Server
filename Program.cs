using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using SparkFlow.Server.Api.DependencyInjection;
using SparkFlow.Server.Api.Endpoints;
using SparkFlow.Server.Api.Middleware;
using SparkFlow.Server.Api.OpenApi;
using SparkFlow.Server.Application.Features.Sessions.CompleteSession;
using SparkFlow.Server.Application.Features.Sessions.FailSession;
using SparkFlow.Server.Application.Features.Sessions.GetSession;
using SparkFlow.Server.Application.Features.Sessions.StartSession;
using SparkFlow.Server.Application.Features.Workers.Heartbeat;
using SparkFlow.Server.Application.Features.Workers.RegisterWorker;
using SparkFlow.Server.Application.Features.Workers.RequestSession;
using SparkFlow.Server.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Keep original property names in JSON payloads.
// This is useful when contracts are already designed with PascalCase.
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = null;
});

builder.Services.AddSparkFlowServer(builder.Configuration);
builder.Services.AddSparkFlowSwagger();

// Register validators and handlers used by the worker/session endpoints.
// These remain singleton because they are stateless.
builder.Services.AddSingleton<RegisterWorkerValidator>();
builder.Services.AddSingleton<RegisterWorkerHandler>();
builder.Services.AddSingleton<WorkerHeartbeatValidator>();
builder.Services.AddSingleton<WorkerHeartbeatHandler>();
builder.Services.AddSingleton<RequestSessionValidator>();
builder.Services.AddSingleton<RequestSessionHandler>();
builder.Services.AddSingleton<StartSessionHandler>();
builder.Services.AddSingleton<CompleteSessionHandler>();
builder.Services.AddSingleton<FailSessionHandler>();
builder.Services.AddSingleton<GetSessionHandler>();

var app = builder.Build();

// Global middleware pipeline.
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ApiKeyMiddleware>();
app.UseMiddleware<WorkerAuthMiddleware>();

// Health endpoints.
app.MapGet("/", () => Results.Ok(new
{
    ok = true,
    name = "SparkFlow.Server",
    utc = DateTime.UtcNow
}));

app.MapGet("/health", () => Results.Ok(new
{
    status = "healthy",
    utc = DateTime.UtcNow
}));

// API endpoints.
app.MapBootstrapEndpoints();
app.MapFlowEndpoints();
app.MapLogEndpoints();
app.MapMetricsEndpoints();
app.MapPolicyEndpoints();
app.MapAccountEndpoints();
app.MapSessionEndpoints();
app.MapWorkerEndpoints();

// Apply EF Core migrations automatically on startup if a connection string exists.
// This keeps local/dev/preview deployments simple.
using (var scope = app.Services.CreateScope())
{
    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");

    if (!string.IsNullOrWhiteSpace(connectionString))
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<SparkFlowDbContext>();
        dbContext.Database.Migrate();
    }
}

app.Run();