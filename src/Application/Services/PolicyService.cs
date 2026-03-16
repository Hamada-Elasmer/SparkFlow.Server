using SparkFlow.Server.Application.Abstractions.Persistence;
using SparkFlow.Server.Domain.Entities;
using SparkFlow.Server.Domain.Rules;

namespace SparkFlow.Server.Application.Services;

public sealed class PolicyService
{
    private readonly IPolicyRepository _repository;

    public PolicyService(IPolicyRepository repository)
    {
        _repository = repository;
    }

    public Task<Policy> GetDefaultAsync(CancellationToken cancellationToken = default) => _repository.GetDefaultAsync(cancellationToken);

    public async Task<bool> CanRunAsync(Account account, CancellationToken cancellationToken = default)
    {
        var policy = await _repository.GetDefaultAsync(cancellationToken);
        return policy.IsEnabled && !AccountPolicyRules.ShouldPause(account, policy);
    }
}
