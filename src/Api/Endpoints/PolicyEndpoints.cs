using SparkFlow.Server.Application.Services;

namespace SparkFlow.Server.Api.Endpoints;

public static class PolicyEndpoints
{
    public static IEndpointRouteBuilder MapPolicyEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/policies/default", async (PolicyService service, CancellationToken ct) => Results.Ok(await service.GetDefaultAsync(ct)));
        return app;
    }
}
