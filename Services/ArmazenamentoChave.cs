using System.Security.Cryptography;
using System.Text;

namespace GeradorParChaves.Services
{
    public class ArmazenamentoChave
    {
        public void ArmazenarChavePrivada(string chavePrivada, string caminhoArquivo)
        {
            byte[] chavePrivadaCriptografada = CriptografarChavePrivada(chavePrivada);
            File.WriteAllBytes(caminhoArquivo, chavePrivadaCriptografada);
        }

        private byte[] CriptografarChavePrivada(string chavePrivada)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = GerarChaveSecreta();
                aes.IV = new byte[16];

                using (var criptografador = aes.CreateEncryptor())
                {
                    byte[] chavePrivadaBytes = Encoding.UTF8.GetBytes(chavePrivada);
                    return criptografador.TransformFinalBlock(chavePrivadaBytes, 0, chavePrivadaBytes.Length);
                }
            }
        }

        private byte[] GerarChaveSecreta()
        {
            return new byte[32];
        }
    }
}
