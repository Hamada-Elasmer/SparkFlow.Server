namespace SparkFlow.Server.Application.Abstractions.Crypto;

/// <summary>
/// Abstraction for hashing service.
/// </summary>
public interface IHasher
{
    string ComputeSha256(string input);
}