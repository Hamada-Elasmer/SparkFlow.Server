namespace SparkFlow.Server.Contracts.Common;

public sealed record PagingRequest(int Page = 1, int PageSize = 50);
