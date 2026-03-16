namespace SparkFlow.Server.Contracts.Common;

public sealed record ApiResponse<T>(bool Success, T? Data, string? Error = null);
