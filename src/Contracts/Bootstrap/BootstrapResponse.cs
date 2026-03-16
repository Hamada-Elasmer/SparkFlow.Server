using SparkFlow.Server.Contracts.Flows;

namespace SparkFlow.Server.Contracts.Bootstrap;

public sealed record BootstrapResponse(string ServerUtc, string Message, FlowDto? ActiveFlow);
