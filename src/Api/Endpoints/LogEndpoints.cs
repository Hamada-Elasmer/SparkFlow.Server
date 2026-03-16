using SparkFlow.Server.Application.Abstractions.Persistence;
using SparkFlow.Server.Contracts.Logs;
using SparkFlow.Server.Domain.Entities;

namespace SparkFlow.Server.Api.Endpoints;

public static class LogEndpoints
{
    public static IEndpointRouteBuilder MapLogEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/logs");
        group.MapPost("/", async (PushLogsRequest request, ILogRepository repository) =>
        {
            var batch = new LogBatch { DeviceId = request.DeviceId, Events = request.Events ?? new List<LogEvent>() };
            await repository.SaveAsync(batch);
            return Results.Ok(new PushLogsResponse(true, batch.Events.Count));
        });
        return app;
    }
}
