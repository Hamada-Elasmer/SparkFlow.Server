using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using SparkFlow.Server.Application.Abstractions.Crypto;

namespace SparkFlow.Server.Infrastructure.Crypto;

public sealed class RsaSigner : ISigner
{
    private readonly RSA? _rsa;

    public RsaSigner(IConfiguration config)
    {
        var privateKeyPem = config["RSA_PRIVATE_KEY"];
        if (!string.IsNullOrWhiteSpace(privateKeyPem))
        {
            _rsa = RSA.Create();
            _rsa.ImportFromPem(privateKeyPem);
        }
    }

    public string Sign(string data)
    {
        if (_rsa is null)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(data));
            return Convert.ToBase64String(bytes);
        }

        var payload = Encoding.UTF8.GetBytes(data);
        var signature = _rsa.SignData(payload, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        return Convert.ToBase64String(signature);
    }
}
