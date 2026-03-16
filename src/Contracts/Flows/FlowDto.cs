namespace SparkFlow.Server.Contracts.Flows;

public sealed record FlowDto(string FlowId, string Sha256, string Signature, DateTime UpdatedUtc);
