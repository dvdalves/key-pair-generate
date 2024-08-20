using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using System.Security.Cryptography;
using System.Text;

namespace GeradorParChaves.Services;

public class ArmazenamentoChave
{
    private readonly string _caminhoArquivo;
    private readonly byte[] _chaveSecreta;
    private readonly byte[] _iv;

    public ArmazenamentoChave(byte[] chaveSecreta, byte[] iv, string caminhoArquivo = "chavePrivada.bson")
    {
        _chaveSecreta = chaveSecreta;
        _iv = iv;
        _caminhoArquivo = caminhoArquivo;
    }

    public void ArmazenarChavePrivada(string chavePrivada, Guid fornecedorId)
    {
        byte[] chavePrivadaCriptografada = CriptografarChavePrivada(chavePrivada);

        var bsonDocument = new BsonDocument
        {
            { "FornecedorId", fornecedorId.ToString() },
            { "chavePrivadaCriptografada", chavePrivadaCriptografada }
        };

        File.WriteAllBytes(_caminhoArquivo, bsonDocument.ToBson());
    }

    public (Guid FornecedorId, string ChavePrivada) BuscarChavePrivada()
    {
        if (!File.Exists(_caminhoArquivo))
        {
            throw new FileNotFoundException("O arquivo de chave privada não foi encontrado.");
        }

        byte[] bsonData = File.ReadAllBytes(_caminhoArquivo);
        var bsonDocument = BsonSerializer.Deserialize<BsonDocument>(bsonData);
        Guid fornecedorId = Guid.Parse(bsonDocument["FornecedorId"].AsString);
        byte[] chavePrivadaCriptografada = bsonDocument["chavePrivadaCriptografada"].AsByteArray;

        string chavePrivada = DescriptografarChavePrivada(chavePrivadaCriptografada);

        return (fornecedorId, chavePrivada);
    }

    private byte[] CriptografarChavePrivada(string chavePrivada)
    {
        using (var aes = Aes.Create())
        {
            aes.Key = _chaveSecreta;
            aes.IV = _iv;

            using (var criptografador = aes.CreateEncryptor())
            {
                byte[] chavePrivadaBytes = Encoding.UTF8.GetBytes(chavePrivada);
                return criptografador.TransformFinalBlock(chavePrivadaBytes, 0, chavePrivadaBytes.Length);
            }
        }
    }

    private string DescriptografarChavePrivada(byte[] chavePrivadaCriptografada)
    {
        using (var aes = Aes.Create())
        {
            aes.Key = _chaveSecreta;
            aes.IV = _iv;

            using (var descriptografador = aes.CreateDecryptor())
            {
                byte[] chavePrivadaBytes = descriptografador.TransformFinalBlock(chavePrivadaCriptografada, 0, chavePrivadaCriptografada.Length);
                return Encoding.UTF8.GetString(chavePrivadaBytes);
            }
        }
    }

    public static byte[] GerarChaveSecreta()
    {
        byte[] key = new byte[32];
        RandomNumberGenerator.Fill(key);
        return key;
    }

    public static byte[] GerarIV()
    {
        byte[] iv = new byte[16];
        RandomNumberGenerator.Fill(iv);
        return iv;
    }
}
