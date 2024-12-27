using KeyPairGenerator.Services;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        var (publicKey, privateKey) = KeyPairGeneratorService.GenerateKeyPair();

        Guid entityId = Guid.NewGuid();
        SecureStorage.StorePrivateKey(privateKey, entityId);

        Console.WriteLine("Generated public key: " + publicKey);

        var (retrievedEntityId, retrievedPrivateKey) = SecureStorage.RetrievePrivateKey(entityId);
        Console.WriteLine($"Entity: {retrievedEntityId}, Retrieved private key: {retrievedPrivateKey}");

        var secret = SecretGenerator.GenerateSecret();
        SecureStorage.StoreSecret(secret, entityId);
        Console.WriteLine("Secret generated and stored.");

        var (retrievedSecretEntityId, retrievedSecret) = SecureStorage.RetrieveSecret(entityId);
        Console.WriteLine("Retrieved secret: " + Convert.ToBase64String(retrievedSecret));

        var encryptedSecret = SecretGenerator.EncryptSecret(retrievedSecret, publicKey);
        Console.WriteLine("Encrypted secret: " + Convert.ToBase64String(encryptedSecret));

        var decryptedSecret = SecretGenerator.DecryptSecret(encryptedSecret, retrievedPrivateKey);
        Console.WriteLine("Decrypted secret: " + Convert.ToBase64String(decryptedSecret));

        byte[] data = Encoding.UTF8.GetBytes("Sensitive data");
        byte[] encryptedData = SecretGenerator.EncryptObject(data, decryptedSecret);
        Console.WriteLine("Encrypted data: " + Convert.ToBase64String(encryptedData));

        byte[] decryptedData = SecretGenerator.DecryptObject(encryptedData, decryptedSecret);
        Console.WriteLine("Decrypted data: " + Encoding.UTF8.GetString(decryptedData));
    }
}
