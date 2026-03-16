using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using SparkFlow.Server.Api.Endpoints;
using SparkFlow.Server.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// JSON options
builder.Services.Configure<JsonOptions>(o =>
{
    o.SerializerOptions.PropertyNamingPolicy = null;
});

// PostgreSQL + EF Core + Npgsql
builder.Services.AddDbContext<SparkFlowDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// JSON Stores الحالية (مؤقتًا لحد ما تستبدلها بـ PostgreSQL Repositories)
builder.Services.AddSingleton<SparkFlow.Server.Infrastructure.Persistence.Json.FlowStore>();
builder.Services.AddSingleton<SparkFlow.Server.Infrastructure.Persistence.Json.LogStore>();
builder.Services.AddSingleton<SparkFlow.Server.Infrastructure.Persistence.Json.UpdateStore>();

var app = builder.Build();

// شغل migration تلقائيًا عند startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SparkFlowDbContext>();
    db.Database.Migrate();
}

// API Key protection
app.Use(async (ctx, next) =>
{
    var apiKey = app.Configuration["API_KEY"];

    if (!string.IsNullOrWhiteSpace(apiKey))
    {
        if (ctx.Request.Path == "/")
        {
            await next();
            return;
        }

        if (!ctx.Request.Headers.TryGetValue("X-Api-Key", out var provided) ||
            !string.Equals(provided.ToString(), apiKey, StringComparison.Ordinal))
        {
            ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await ctx.Response.WriteAsJsonAsync(new { error = "unauthorized" });
            return;
        }
    }

    await next();
});

app.MapGet("/", () => Results.Ok(new
{
    ok = true,
    name = "SparkFlow.Server",
    utc = DateTime.UtcNow
}));

app.MapGet("/health", () => Results.Ok(new
{
    status = "healthy",
    utc = DateTime.UtcNow
}));

// Endpoints
app.MapFlowEndpoints();
app.MapLogEndpoints();
app.MapBootstrapEndpoints();

app.Run();