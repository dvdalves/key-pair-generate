using System.Security.Cryptography;

namespace KeyPairGenerator.Services;

public class KeyPairGeneratorService
{
    public static (string PublicKey, string PrivateKey) GenerateKeyPair()
    {
        using var rsa = RSA.Create(4096);
        string publicKey = Convert.ToBase64String(rsa.ExportRSAPublicKey());
        string privateKey = Convert.ToBase64String(rsa.ExportRSAPrivateKey());

        return (publicKey, privateKey);
    }
}
