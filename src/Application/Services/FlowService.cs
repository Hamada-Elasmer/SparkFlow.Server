using SparkFlow.Server.Application.Abstractions.Persistence;
using SparkFlow.Server.Domain.Entities;

namespace SparkFlow.Server.Application.Services;

public sealed class FlowService
{
    private readonly IFlowRepository _flowRepository;

    public FlowService(IFlowRepository flowRepository)
    {
        _flowRepository = flowRepository;
    }

    public FlowEnvelope? Get(string flowId) => _flowRepository.Get(flowId);

    public IReadOnlyList<FlowEnvelope> List(IEnumerable<string>? flowIds = null)
    {
        var ids = flowIds?.ToList() ?? new List<string> { "daily_run" };
        return ids.Select(id => _flowRepository.Get(id)).Where(x => x is not null).Cast<FlowEnvelope>().ToList();
    }

    public void Publish(string flowId, string json) => _flowRepository.Save(flowId, json);
}
