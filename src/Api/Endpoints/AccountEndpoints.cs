using SparkFlow.Server.Api.Mapping;
using SparkFlow.Server.Application.Services;
using SparkFlow.Server.Contracts.Accounts;

namespace SparkFlow.Server.Api.Endpoints;

public static class AccountEndpoints
{
    public static IEndpointRouteBuilder MapAccountEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/accounts");
        group.MapGet("/", async (AccountService service, CancellationToken ct) => Results.Ok((await service.ListAsync(ct)).Select(x => x.ToDto())));
        group.MapGet("/{id}", async (string id, AccountService service, CancellationToken ct) =>
        {
            var account = await service.GetAsync(id, ct);
            return account is null ? Results.NotFound() : Results.Ok(account.ToDto());
        });
        group.MapPost("/", async (CreateAccountRequest request, AccountService service, CancellationToken ct) => Results.Ok((await service.CreateAsync(request.GameId, request.NextRunAtUtc, ct)).ToDto()));
        group.MapPut("/{id}", async (string id, UpdateAccountRequest request, AccountService service, CancellationToken ct) =>
        {
            var account = await service.UpdateAsync(id, request.GameId, request.Status, request.NextRunAtUtc, ct);
            return account is null ? Results.NotFound() : Results.Ok(account.ToDto());
        });
        group.MapPost("/{id}/unlock", async (string id, AccountService service, CancellationToken ct) => { await service.UnlockAsync(id, ct); return Results.Ok(); });
        return app;
    }
}
