using GeradorParChaves.Services;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        var geradorParChaves = new GeradorParChave();

        // Gerar par de chaves RSA
        var (chavePublica, chavePrivada) = geradorParChaves.GerarParChaves();

        // Simular o ID de um fornecedor
        Guid fornecedorId = Guid.NewGuid();

        // Armazenar a chave privada no armazenamento seguro
        Armazenamento.ArmazenarChavePrivada(chavePrivada, fornecedorId);

        Console.WriteLine("Chave pública gerada: " + chavePublica);

        // Recuperar a chave privada para uso posterior
        var (fornecedorIdRecuperado, chavePrivadaRecuperada) = Armazenamento.BuscarChavePrivada(fornecedorId);
        Console.WriteLine($"Fornecedor: {fornecedorIdRecuperado}, Chave privada recuperada: {chavePrivadaRecuperada}");

        // Gerar um segredo (chave simétrica)
        var segredo = GeradorSegredo.GerarSegredo();

        // Armazenar o segredo
        Armazenamento.ArmazenarSegredo(segredo, fornecedorId);
        Console.WriteLine("Segredo gerado e armazenado.");

        // Recuperar o segredo para uso posterior
        var (fornecedorIdSegredoRecuperado, segredoRecuperado) = Armazenamento.BuscarSegredo(fornecedorId);
        Console.WriteLine("Segredo recuperado: " + Convert.ToBase64String(segredoRecuperado));

        // Criptografar o segredo com a chave pública RSA
        var segredoCriptografado = GeradorSegredo.CriptografarSegredo(segredoRecuperado, chavePublica);
        Console.WriteLine("Segredo criptografado: " + Convert.ToBase64String(segredoCriptografado));

        // Descriptografar o segredo com a chave privada RSA
        var segredoDescriptografado = GeradorSegredo.DescriptografarSegredo(segredoCriptografado, chavePrivadaRecuperada);
        Console.WriteLine("Segredo descriptografado: " + Convert.ToBase64String(segredoDescriptografado));

        // Exemplo de uso do segredo descriptografado para criptografar um objeto (dados)
        byte[] dados = Encoding.UTF8.GetBytes("Dados sensíveis");
        byte[] dadosCriptografados = GeradorSegredo.CriptografarObjeto(dados, segredoDescriptografado);
        Console.WriteLine("Dados criptografados: " + Convert.ToBase64String(dadosCriptografados));

        // Descriptografar os dados
        byte[] dadosDescriptografados = GeradorSegredo.DescriptografarObjeto(dadosCriptografados, segredoDescriptografado);
        Console.WriteLine("Dados descriptografados: " + Encoding.UTF8.GetString(dadosDescriptografados));
    }
}
