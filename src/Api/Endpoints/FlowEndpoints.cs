using SparkFlow.Server.Api.Mapping;
using SparkFlow.Server.Application.Services;
using SparkFlow.Server.Contracts.Flows;

namespace SparkFlow.Server.Api.Endpoints;

public static class FlowEndpoints
{
    public static IEndpointRouteBuilder MapFlowEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/flows");
        group.MapGet("/{flowId}", (string flowId, FlowService service) =>
        {
            var flow = service.Get(flowId);
            return flow is null ? Results.NotFound() : Results.Ok(new GetFlowResponse(flow.ToDefinitionDto()));
        });
        group.MapGet("/", (FlowService service) => Results.Ok(service.List().Select(x => x.ToDto())));
        group.MapPost("/", (CreateFlowRequest request, FlowService service) => { service.Publish(request.FlowId, request.Json); return Results.Ok(); });
        return app;
    }
}
