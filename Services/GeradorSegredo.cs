using System.Security.Cryptography;
using System.IO;

namespace GeradorParChaves.Services
{
    public static class GeradorSegredo
    {
        public static byte[] GerarSegredo()
        {
            byte[] segredo = new byte[32]; // Tamanho do segredo pode ser ajustado conforme necessidade
            RandomNumberGenerator.Fill(segredo);
            return segredo;
        }

        public static byte[] CriptografarSegredo(byte[] segredo, string chavePublicaBase64)
        {
            using (var rsa = RSA.Create())
            {
                rsa.ImportRSAPublicKey(Convert.FromBase64String(chavePublicaBase64), out _);
                return rsa.Encrypt(segredo, RSAEncryptionPadding.OaepSHA256);
            }
        }

        public static byte[] DescriptografarSegredo(byte[] segredoCriptografado, string chavePrivadaBase64)
        {
            using (var rsa = RSA.Create())
            {
                rsa.ImportRSAPrivateKey(Convert.FromBase64String(chavePrivadaBase64), out _);
                return rsa.Decrypt(segredoCriptografado, RSAEncryptionPadding.OaepSHA256);
            }
        }

        public static byte[] CriptografarObjeto(byte[] objeto, byte[] segredo)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = segredo;
                aes.GenerateIV();

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream())
                {
                    ms.Write(aes.IV, 0, aes.IV.Length);
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(objeto, 0, objeto.Length);
                        cs.FlushFinalBlock();
                    }

                    return ms.ToArray();
                }
            }
        }

        public static byte[] DescriptografarObjeto(byte[] objetoCriptografado, byte[] segredo)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = segredo;

                using (var ms = new MemoryStream(objetoCriptografado))
                {
                    byte[] iv = new byte[16];
                    ms.Read(iv, 0, iv.Length);
                    aes.IV = iv;

                    using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    using (var resultStream = new MemoryStream())
                    {
                        cs.CopyTo(resultStream);
                        return resultStream.ToArray();
                    }
                }
            }
        }

        public static void CriptografarArquivo(string caminhoArquivo, byte[] segredo)
        {
            byte[] arquivoBytes = File.ReadAllBytes(caminhoArquivo);
            byte[] arquivoCriptografado = CriptografarObjeto(arquivoBytes, segredo);
            File.WriteAllBytes(caminhoArquivo, arquivoCriptografado);
        }

        public static void DescriptografarArquivo(string caminhoArquivo, byte[] segredo)
        {
            byte[] arquivoCriptografado = File.ReadAllBytes(caminhoArquivo);
            byte[] arquivoDescriptografado = DescriptografarObjeto(arquivoCriptografado, segredo);
            File.WriteAllBytes(caminhoArquivo, arquivoDescriptografado);
        }
    }
}