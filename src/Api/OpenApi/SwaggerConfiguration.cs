namespace SparkFlow.Server.Api.OpenApi;

public static class SwaggerConfiguration
{
    public static IServiceCollection AddSparkFlowSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        return services;
    }
}
