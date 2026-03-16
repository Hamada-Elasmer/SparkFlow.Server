using System.Text.Json;
using System.Text.Json.Serialization;

namespace SparkFlow.Server.Infrastructure.Serialization;

public static class JsonSerializerOptionsFactory
{
    public static JsonSerializerOptions Create() => new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = null,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };
}
