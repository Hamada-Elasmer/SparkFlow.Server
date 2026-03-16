using SparkFlow.Server.Application.Services;

namespace SparkFlow.Server.Api.Endpoints;

public static class MetricsEndpoints
{
    public static IEndpointRouteBuilder MapMetricsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/metrics", (MetricsService service) => Results.Ok(service.Snapshot()));
        return app;
    }
}
