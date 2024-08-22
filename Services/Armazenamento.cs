using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using System.Security.Cryptography;
using System.Text;

namespace GeradorParChaves.Services;

public static class Armazenamento
{
    private const string CaminhoArquivoChave = "chavePrivada.bson";
    private const string CaminhoArquivoSegredo = "segredo.bson";
    private static readonly byte[] ChaveSecreta = GerarChaveSecreta();
    private static readonly byte[] Iv = GerarIV();

    public static void ArmazenarChavePrivada(string chavePrivada, Guid fornecedorId)
    {
        // A chave privada é armazenada criptografada
        byte[] chavePrivadaCriptografada = Criptografar(chavePrivada);

        // O arquivo de chave privada é armazenado em BSON
        var bsonDocument = new BsonDocument
        {
            { "FornecedorId", fornecedorId.ToString() },
            { "chavePrivadaCriptografada", chavePrivadaCriptografada }
        };

        // O arquivo é salvo no disco
        File.WriteAllBytes(CaminhoArquivoChave, bsonDocument.ToBson());
    }

    public static (Guid FornecedorId, string ChavePrivada) BuscarChavePrivada(Guid fornecedorId)
    {
        if (!File.Exists(CaminhoArquivoChave))
        {
            throw new FileNotFoundException("O arquivo de chave privada não foi encontrado.");
        }

        // O arquivo de chave privada é lido do disco
        byte[] bsonData = File.ReadAllBytes(CaminhoArquivoChave);
        var bsonDocument = BsonSerializer.Deserialize<BsonDocument>(bsonData);
        // O FornecedorId é recuperado do arquivo
        Guid fornecedorIdRecuperado = Guid.Parse(bsonDocument["FornecedorId"].AsString);

        if (fornecedorIdRecuperado != fornecedorId)
        {
            throw new UnauthorizedAccessException("FornecedorId não autorizado.");
        }

        // A chave privada é recuperada do arquivo e descriptografada
        byte[] chavePrivadaCriptografada = bsonDocument["chavePrivadaCriptografada"].AsByteArray;
        string chavePrivada = Descriptografar(chavePrivadaCriptografada);

        return (fornecedorIdRecuperado, chavePrivada);
    }


    // Método de criptografia comum
    private static byte[] Criptografar(string texto)
    {
        using (var aes = Aes.Create())
        {
            aes.Key = ChaveSecreta;
            aes.IV = Iv;

            using (var criptografador = aes.CreateEncryptor())
            {
                byte[] textoBytes = Encoding.UTF8.GetBytes(texto);
                return criptografador.TransformFinalBlock(textoBytes, 0, textoBytes.Length);
            }
        }
    }

    // Método de descriptografia comum
    private static string Descriptografar(byte[] textoCriptografado)
    {
        using (var aes = Aes.Create())
        {
            aes.Key = ChaveSecreta;
            aes.IV = Iv;

            using (var descriptografador = aes.CreateDecryptor())
            {
                byte[] textoBytes = descriptografador.TransformFinalBlock(textoCriptografado, 0, textoCriptografado.Length);
                return Encoding.UTF8.GetString(textoBytes);
            }
        }
    }

    // Método para gerar chave secreta
    private static byte[] GerarChaveSecreta()
    {
        byte[] key = new byte[32];
        RandomNumberGenerator.Fill(key);
        return key;
    }

    // IV (Initialization Vector) é um vetor de inicialização que é usado para garantir que duas mensagens criptografadas não sejam iguais
    private static byte[] GerarIV()
    {
        byte[] iv = new byte[16];
        RandomNumberGenerator.Fill(iv);
        return iv;
    }

    // Armazenamento de Segredo
    public static void ArmazenarSegredo(byte[] segredo, Guid fornecedorId)
    {
        byte[] segredoCriptografado = Criptografar(Convert.ToBase64String(segredo));

        var bsonDocument = new BsonDocument
        {
            { "FornecedorId", fornecedorId.ToString() },
            { "segredoCriptografado", segredoCriptografado }
        };

        File.WriteAllBytes(CaminhoArquivoSegredo, bsonDocument.ToBson());
    }

    // Busca de Segredo
    public static (Guid FornecedorId, byte[] Segredo) BuscarSegredo(Guid fornecedorId)
    {
        if (!File.Exists(CaminhoArquivoSegredo))
        {
            throw new FileNotFoundException("O arquivo de segredo não foi encontrado.");
        }

        byte[] bsonData = File.ReadAllBytes(CaminhoArquivoSegredo);
        var bsonDocument = BsonSerializer.Deserialize<BsonDocument>(bsonData);
        Guid fornecedorIdRecuperado = Guid.Parse(bsonDocument["FornecedorId"].AsString);

        if (fornecedorIdRecuperado != fornecedorId)
        {
            throw new UnauthorizedAccessException("FornecedorId não autorizado.");
        }

        byte[] segredoCriptografado = bsonDocument["segredoCriptografado"].AsByteArray;
        string segredoDescriptografado = Descriptografar(segredoCriptografado);

        return (fornecedorIdRecuperado, Convert.FromBase64String(segredoDescriptografado));
    }
}