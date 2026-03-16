using SparkFlow.Server.Contracts.Common;

namespace SparkFlow.Server.Api.Middleware;

public sealed class WorkerAuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public WorkerAuthMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Path.StartsWithSegments("/workers"))
        {
            await _next(context);
            return;
        }

        var required = _configuration["WORKER_TOKEN"];
        if (string.IsNullOrWhiteSpace(required))
        {
            await _next(context);
            return;
        }

        var provided = context.Request.Headers.Authorization.ToString().Replace("Bearer ", string.Empty, StringComparison.OrdinalIgnoreCase);
        if (!string.Equals(required, provided, StringComparison.Ordinal))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new ErrorResponse("worker_unauthorized", context.TraceIdentifier));
            return;
        }

        await _next(context);
    }
}
