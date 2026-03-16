using SparkFlow.Server.Contracts.Common;

namespace SparkFlow.Server.Api.Middleware;

public sealed class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var apiKey = _configuration["API_KEY"];
        if (string.IsNullOrWhiteSpace(apiKey) || context.Request.Path == "/" || context.Request.Path.StartsWithSegments("/health"))
        {
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue("X-Api-Key", out var provided) || !string.Equals(provided.ToString(), apiKey, StringComparison.Ordinal))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new ErrorResponse("unauthorized", context.TraceIdentifier));
            return;
        }

        await _next(context);
    }
}
