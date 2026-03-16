using SparkFlow.Server.Contracts.Accounts;
using SparkFlow.Server.Contracts.Flows;
using SparkFlow.Server.Contracts.Sessions;

namespace SparkFlow.Server.Contracts.Workers;

public sealed record RequestSessionResponse(bool HasWork, SessionDto? Session, AccountDto? Account, FlowDefinitionDto? Flow);
