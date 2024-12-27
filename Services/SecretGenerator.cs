using System.Security.Cryptography;

namespace KeyPairGenerator.Services;

public static class SecretGenerator
{
    public static byte[] GenerateSecret()
    {
        byte[] secret = new byte[32];
        RandomNumberGenerator.Fill(secret);
        return secret;
    }

    public static byte[] EncryptSecret(byte[] secret, string publicKeyBase64)
    {
        using var rsa = RSA.Create();
        rsa.ImportRSAPublicKey(Convert.FromBase64String(publicKeyBase64), out _);
        return rsa.Encrypt(secret, RSAEncryptionPadding.OaepSHA256);
    }

    public static byte[] DecryptSecret(byte[] encryptedSecret, string privateKeyBase64)
    {
        using var rsa = RSA.Create();
        rsa.ImportRSAPrivateKey(Convert.FromBase64String(privateKeyBase64), out _);
        return rsa.Decrypt(encryptedSecret, RSAEncryptionPadding.OaepSHA256);
    }

    public static byte[] EncryptObject(byte[] data, byte[] secret)
    {
        using var aes = Aes.Create();
        aes.Key = secret;
        aes.GenerateIV();

        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream();
        ms.Write(aes.IV, 0, aes.IV.Length);
        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        {
            cs.Write(data, 0, data.Length);
            cs.FlushFinalBlock();
        }

        return ms.ToArray();
    }

    public static byte[] DecryptObject(byte[] encryptedData, byte[] secret)
    {
        using var aes = Aes.Create();
        aes.Key = secret;

        using var ms = new MemoryStream(encryptedData);
        byte[] iv = new byte[16];
        ms.Read(iv, 0, iv.Length);
        aes.IV = iv;

        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var resultStream = new MemoryStream();
        cs.CopyTo(resultStream);
        return resultStream.ToArray();
    }
}
