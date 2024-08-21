using BenchmarkDotNet.Attributes;
using System.Security.Cryptography;
using System.Text;

namespace GeradorParChaves.Benchmarks;

public class CriptografiaBenchmark
{
    private readonly byte[] _chavePrivada;
    private readonly byte[] _chaveSecreta;
    private readonly byte[] _iv;
    private readonly Aes _aes;

    public CriptografiaBenchmark()
    {
        _chavePrivada = Encoding.UTF8.GetBytes("Esta é uma chave privada de teste.");
        _chaveSecreta = new byte[32];
        _iv = new byte[16];
        RandomNumberGenerator.Fill(_chaveSecreta);
        RandomNumberGenerator.Fill(_iv);

        _aes = Aes.Create();
        _aes.Key = _chaveSecreta;
        _aes.IV = _iv;
    }

    [Benchmark]
    public byte[] CriptografarChavePrivada()
    {
        using (var criptografador = _aes.CreateEncryptor())
        {
            return criptografador.TransformFinalBlock(_chavePrivada, 0, _chavePrivada.Length);
        }
    }

    [Benchmark]
    public string DescriptografarChavePrivada()
    {
        byte[] chavePrivadaCriptografada = CriptografarChavePrivada();
        using (var descriptografador = _aes.CreateDecryptor())
        {
            byte[] chavePrivadaBytes = descriptografador.TransformFinalBlock(chavePrivadaCriptografada, 0, chavePrivadaCriptografada.Length);
            return Encoding.UTF8.GetString(chavePrivadaBytes);
        }
    }
}
