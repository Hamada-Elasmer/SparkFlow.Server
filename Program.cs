using Microsoft.AspNetCore.Http.Json;
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

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JsonOptions>(o =>
{
    o.SerializerOptions.PropertyNamingPolicy = null;
});

builder.Services.AddSparkFlowServer(builder.Configuration);
builder.Services.AddSparkFlowSwagger();

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

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ApiKeyMiddleware>();
app.UseMiddleware<WorkerAuthMiddleware>();


app.MapGet("/", () => Results.Ok(new { ok = true, name = "SparkFlow.Server", utc = DateTime.UtcNow }));
app.MapGet("/health", () => Results.Ok(new { status = "healthy", utc = DateTime.UtcNow }));

app.MapBootstrapEndpoints();
app.MapFlowEndpoints();
app.MapLogEndpoints();
app.MapMetricsEndpoints();
app.MapPolicyEndpoints();
app.MapAccountEndpoints();
app.MapSessionEndpoints();
app.MapWorkerEndpoints();

app.Run();
