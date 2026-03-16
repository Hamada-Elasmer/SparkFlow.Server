namespace SparkFlow.Server.Contracts.Common;

public sealed record ErrorResponse(string Error, string? CorrelationId = null);
