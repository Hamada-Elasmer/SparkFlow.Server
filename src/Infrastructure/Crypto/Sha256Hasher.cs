using System;
using System.Security.Cryptography;
using System.Text;
using SparkFlow.Server.Application.Abstractions.Crypto;

namespace SparkFlow.Server.Infrastructure.Crypto;

/// <summary>
/// SHA256 hashing implementation.
/// </summary>
public sealed class Sha256Hasher : IHasher
{
    public string ComputeSha256(string input)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}