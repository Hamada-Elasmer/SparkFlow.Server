using SparkFlow.Server.Api.Mapping;
using SparkFlow.Server.Application.Services;
using SparkFlow.Server.Contracts.Bootstrap;

namespace SparkFlow.Server.Api.Endpoints;

public static class BootstrapEndpoints
{
    public static IEndpointRouteBuilder MapBootstrapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/bootstrap", (FlowService flowService) =>
        {
            var flow = flowService.Get("daily_run");
            var response = new BootstrapResponse(DateTime.UtcNow.ToString("O"), "ok", flow?.ToDto());
            return Results.Ok(response);
        });
        return app;
    }
}
