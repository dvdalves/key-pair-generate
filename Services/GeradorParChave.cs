using System.Security.Cryptography;

namespace GeradorParChaves.Services;

public class GeradorParChave
{
    public (string chavePublica, string chavePrivada) GerarParChaves()
    {
        using (var rsa = RSA.Create(4096))
        {
            string chavePublica = Convert.ToBase64String(rsa.ExportRSAPublicKey());
            string chavePrivada = Convert.ToBase64String(rsa.ExportRSAPrivateKey());

            return (chavePublica, chavePrivada);
        }
    }
}