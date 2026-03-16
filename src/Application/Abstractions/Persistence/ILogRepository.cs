using System.Threading.Tasks;
using SparkFlow.Server.Domain.Entities;

namespace SparkFlow.Server.Application.Abstractions.Persistence;

public interface ILogRepository
{
    Task SaveAsync(LogBatch batch);
}