using SparkFlow.Server.Application.Abstractions.Crypto;
using SparkFlow.Server.Application.Abstractions.Persistence;
using SparkFlow.Server.Domain.Entities;

namespace SparkFlow.Server.Infrastructure.Persistence.Repositories;

public sealed class JsonFlowRepository : IFlowRepository
{
    private readonly IHasher _hasher;
    private readonly ISigner _signer;
    private const string StorageFolder = "data/flows";

    public JsonFlowRepository(IHasher hasher, ISigner signer)
    {
        _hasher = hasher;
        _signer = signer;
        Directory.CreateDirectory(StorageFolder);
    }

    public FlowEnvelope? Get(string flowId)
    {
        var path = ResolvePath(flowId);
        if (!File.Exists(path)) return null;

        var json = File.ReadAllText(path);
        return new FlowEnvelope
        {
            FlowId = flowId,
            Json = json,
            Sha256 = _hasher.ComputeSha256(json),
            Signature = _signer.Sign(json),
            UpdatedUtc = File.GetLastWriteTimeUtc(path)
        };
    }

    public void Save(string flowId, string json)
    {
        var path = Path.Combine(StorageFolder, $"{flowId}.json");
        File.WriteAllText(path, json);
    }

    private static string ResolvePath(string flowId)
    {
        var dedicated = Path.Combine(StorageFolder, $"{flowId}.json");
        if (File.Exists(dedicated)) return dedicated;

        var rootFallback = Path.Combine(AppContext.BaseDirectory, "flow.json");
        if (File.Exists(rootFallback)) return rootFallback;

        return dedicated;
    }
}
