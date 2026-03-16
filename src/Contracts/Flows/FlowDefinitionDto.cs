namespace SparkFlow.Server.Contracts.Flows;

public sealed record FlowDefinitionDto(string FlowId, string Json, string Sha256, string Signature, DateTime UpdatedUtc);
