using SparkFlow.Server.Domain.Entities;

namespace SparkFlow.Server.Application.Scheduling.Selection;

public sealed record SchedulingDecision(Account Account, WorkerNode Worker);
