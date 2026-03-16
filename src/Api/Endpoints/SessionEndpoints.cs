using SparkFlow.Server.Api.Mapping;
using SparkFlow.Server.Application.Features.Sessions.CompleteSession;
using SparkFlow.Server.Application.Features.Sessions.FailSession;
using SparkFlow.Server.Application.Features.Sessions.GetSession;
using SparkFlow.Server.Application.Features.Sessions.StartSession;
using SparkFlow.Server.Application.Services;
using SparkFlow.Server.Contracts.Sessions;

namespace SparkFlow.Server.Api.Endpoints;

public static class SessionEndpoints
{
    public static IEndpointRouteBuilder MapSessionEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/sessions");
        group.MapGet("/", async (SessionService service, CancellationToken ct) => Results.Ok((await service.ListAsync(ct)).Select(x => x.ToDto())));
        group.MapGet("/{id}", async (string id, GetSessionHandler handler, CancellationToken ct) =>
        {
            var session = await handler.Handle(new GetSessionQuery(id), ct);
            return session is null ? Results.NotFound() : Results.Ok(session.ToDto());
        });
        group.MapPost("/{id}/started", async (string id, SessionStartedRequest _, StartSessionHandler handler, CancellationToken ct) => (await handler.Handle(new StartSessionCommand(id), ct)) ? Results.Ok() : Results.NotFound());
        group.MapPost("/{id}/completed", async (string id, SessionCompletedRequest _, CompleteSessionHandler handler, CancellationToken ct) => (await handler.Handle(new CompleteSessionCommand(id), ct)) ? Results.Ok() : Results.NotFound());
        group.MapPost("/{id}/failed", async (string id, SessionFailedRequest request, FailSessionHandler handler, CancellationToken ct) => (await handler.Handle(new FailSessionCommand(id, request.Error), ct)) ? Results.Ok() : Results.NotFound());
        return app;
    }
}
