using SparkFlow.Server.Contracts.Flows;
using SparkFlow.Server.Domain.Entities;

namespace SparkFlow.Server.Api.Mapping;

public static class FlowMappings
{
    public static FlowDto ToDto(this FlowEnvelope flow) => new(flow.FlowId, flow.Sha256, flow.Signature, flow.UpdatedUtc);
    public static FlowDefinitionDto ToDefinitionDto(this FlowEnvelope flow) => new(flow.FlowId, flow.Json, flow.Sha256, flow.Signature, flow.UpdatedUtc);
}
