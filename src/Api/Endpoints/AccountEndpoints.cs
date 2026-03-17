using SparkFlow.Server.Api.Mapping;
using SparkFlow.Server.Application.Services;
using SparkFlow.Server.Contracts.Accounts;

namespace SparkFlow.Server.Api.Endpoints;

/// <summary>
/// HTTP endpoints for account management.
/// These endpoints are repository-agnostic and continue to work
/// whether persistence is JSON-based or PostgreSQL-based.
/// </summary>
public static class AccountEndpoints
{
    public static IEndpointRouteBuilder MapAccountEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/accounts");

        group.MapGet("/", async (AccountService service, CancellationToken ct) =>
        {
            var accounts = await service.ListAsync(ct);
            return Results.Ok(accounts.Select(x => x.ToDto()));
        });

        group.MapGet("/{id}", async (string id, AccountService service, CancellationToken ct) =>
        {
            var account = await service.GetAsync(id, ct);
            return account is null ? Results.NotFound() : Results.Ok(account.ToDto());
        });

        group.MapPost("/", async (CreateAccountRequest request, AccountService service, CancellationToken ct) =>
        {
            var account = await service.CreateAsync(request.GameId, request.NextRunAtUtc, ct);
            return Results.Ok(account.ToDto());
        });

        group.MapPut("/{id}", async (string id, UpdateAccountRequest request, AccountService service, CancellationToken ct) =>
        {
            var account = await service.UpdateAsync(id, request.GameId, request.Status, request.NextRunAtUtc, ct);
            return account is null ? Results.NotFound() : Results.Ok(account.ToDto());
        });

        group.MapPost("/{id}/unlock", async (string id, AccountService service, CancellationToken ct) =>
        {
            await service.UnlockAsync(id, ct);
            return Results.Ok();
        });

        return app;
    }
}