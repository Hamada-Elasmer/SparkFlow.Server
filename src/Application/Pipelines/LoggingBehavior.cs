namespace SparkFlow.Server.Application.Pipelines; public sealed class LoggingBehavior<TRequest,TResponse>{ public Task<TResponse> HandleAsync(TRequest request, Func<Task<TResponse>> next)=>next(); }
