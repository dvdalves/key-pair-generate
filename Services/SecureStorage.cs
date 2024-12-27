using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using System.Security.Cryptography;
using System.Text;

namespace KeyPairGenerator.Services;

public static class SecureStorage
{
    private const string PrivateKeyFilePath = "privateKey.bson";
    private const string SecretFilePath = "secret.bson";
    private static readonly byte[] SecretKey = GenerateSecretKey();
    private static readonly byte[] Iv = GenerateIV();

    public static void StorePrivateKey(string privateKey, Guid entityId)
    {
        byte[] encryptedPrivateKey = Encrypt(privateKey);

        var bsonDocument = new BsonDocument
        {
            { "EntityId", entityId.ToString() },
            { "EncryptedPrivateKey", encryptedPrivateKey }
        };

        File.WriteAllBytes(PrivateKeyFilePath, bsonDocument.ToBson());
    }

    public static (Guid EntityId, string PrivateKey) RetrievePrivateKey(Guid entityId)
    {
        if (!File.Exists(PrivateKeyFilePath))
            throw new FileNotFoundException("The private key file was not found.");

        byte[] bsonData = File.ReadAllBytes(PrivateKeyFilePath);
        var bsonDocument = BsonSerializer.Deserialize<BsonDocument>(bsonData);
        Guid retrievedEntityId = Guid.Parse(bsonDocument["EntityId"].AsString);

        if (retrievedEntityId != entityId)
            throw new UnauthorizedAccessException("Unauthorized EntityId.");

        byte[] encryptedPrivateKey = bsonDocument["EncryptedPrivateKey"].AsByteArray;
        string privateKey = Decrypt(encryptedPrivateKey);

        return (retrievedEntityId, privateKey);
    }

    private static byte[] Encrypt(string data)
    {
        using var aes = Aes.Create();
        aes.Key = SecretKey;
        aes.IV = Iv;

        using var encryptor = aes.CreateEncryptor();
        byte[] dataBytes = Encoding.UTF8.GetBytes(data);
        return encryptor.TransformFinalBlock(dataBytes, 0, dataBytes.Length);
    }

    private static string Decrypt(byte[] encryptedData)
    {
        using var aes = Aes.Create();
        aes.Key = SecretKey;
        aes.IV = Iv;

        using var decryptor = aes.CreateDecryptor();
        byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
        return Encoding.UTF8.GetString(decryptedBytes);
    }

    private static byte[] GenerateSecretKey()
    {
        byte[] key = new byte[32];
        RandomNumberGenerator.Fill(key);
        return key;
    }

    private static byte[] GenerateIV()
    {
        byte[] iv = new byte[16];
        RandomNumberGenerator.Fill(iv);
        return iv;
    }

    public static void StoreSecret(byte[] secret, Guid entityId)
    {
        byte[] encryptedSecret = Encrypt(Convert.ToBase64String(secret));

        var bsonDocument = new BsonDocument
        {
            { "EntityId", entityId.ToString() },
            { "EncryptedSecret", encryptedSecret }
        };

        File.WriteAllBytes(SecretFilePath, bsonDocument.ToBson());
    }

    public static (Guid EntityId, byte[] Secret) RetrieveSecret(Guid entityId)
    {
        if (!File.Exists(SecretFilePath))
            throw new FileNotFoundException("The secret file was not found.");

        byte[] bsonData = File.ReadAllBytes(SecretFilePath);
        var bsonDocument = BsonSerializer.Deserialize<BsonDocument>(bsonData);
        Guid retrievedEntityId = Guid.Parse(bsonDocument["EntityId"].AsString);

        if (retrievedEntityId != entityId)
            throw new UnauthorizedAccessException("Unauthorized EntityId.");

        byte[] encryptedSecret = bsonDocument["EncryptedSecret"].AsByteArray;
        string decryptedSecret = Decrypt(encryptedSecret);

        return (retrievedEntityId, Convert.FromBase64String(decryptedSecret));
    }
}
