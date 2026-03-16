using SparkFlow.Server.Api.Mapping;
using SparkFlow.Server.Application.Features.Workers.Heartbeat;
using SparkFlow.Server.Application.Features.Workers.RegisterWorker;
using SparkFlow.Server.Application.Features.Workers.RequestSession;
using SparkFlow.Server.Contracts.Workers;

namespace SparkFlow.Server.Api.Endpoints;

public static class WorkerEndpoints
{
    public static IEndpointRouteBuilder MapWorkerEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/workers");
        group.MapPost("/register", async (RegisterWorkerRequest request, RegisterWorkerValidator validator, RegisterWorkerHandler handler, CancellationToken ct) =>
        {
            var command = new RegisterWorkerCommand(request.Name, request.MachineId, request.Version, request.MaxConcurrentSessions);
            if (!validator.Validate(command, out var error)) return Results.BadRequest(error);
            var worker = await handler.Handle(command, ct);
            return Results.Ok(new RegisterWorkerResponse(worker.Id.Value, worker.Status.ToString()));
        });
        group.MapPost("/heartbeat", async (WorkerHeartbeatRequest request, WorkerHeartbeatValidator validator, WorkerHeartbeatHandler handler, CancellationToken ct) =>
        {
            var command = new WorkerHeartbeatCommand(request.WorkerId, request.IpAddress);
            if (!validator.Validate(command, out var error)) return Results.BadRequest(error);
            var worker = await handler.Handle(command, ct);
            return worker is null ? Results.NotFound() : Results.Ok(new WorkerHeartbeatResponse(true, worker.Status.ToString(), DateTime.UtcNow));
        });
        group.MapPost("/request-session", async (RequestSessionRequest request, RequestSessionValidator validator, RequestSessionHandler handler, CancellationToken ct) =>
        {
            var command = new RequestSessionCommand(request.WorkerId);
            if (!validator.Validate(command, out var error)) return Results.BadRequest(error);
            var result = await handler.Handle(command, ct);
            if (result.Session is null || result.Account is null || result.Flow is null) return Results.Ok(new RequestSessionResponse(false, null, null, null));
            return Results.Ok(new RequestSessionResponse(true, result.Session.ToDto(), result.Account.ToDto(), result.Flow.ToDefinitionDto()));
        });
        return app;
    }
}
