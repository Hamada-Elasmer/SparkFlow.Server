namespace SparkFlow.Server.Application.Abstractions.Crypto;

/// <summary>
/// Abstraction for digital signature.
/// </summary>
public interface ISigner
{
    string Sign(string data);
}