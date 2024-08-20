using GeradorParChaves.Services;

class Program
{
    static void Main(string[] args)
    {
        var geradorParChaves = new GeradorParChave();
        var chaveSecreta = ArmazenamentoChave.GerarChaveSecreta();
        var iv = ArmazenamentoChave.GerarIV();

        var armazenamentoChave = new ArmazenamentoChave(chaveSecreta, iv);
        var servicoAuditoria = new ServicoAuditoria();

        var (chavePublica, chavePrivada) = geradorParChaves.GerarParChaves();

        Guid fornecedorId = Guid.NewGuid();
        armazenamentoChave.ArmazenarChavePrivada(chavePrivada, fornecedorId);
        servicoAuditoria.AuditarGeracaoChave("admin");

        Console.WriteLine("Chave pública gerada: " + chavePublica);

        var (fornecedorIdRecuperado, chavePrivadaRecuperada) = armazenamentoChave.BuscarChavePrivada();
        Console.WriteLine($"Fornecedor: {fornecedorIdRecuperado}, Chave privada recuperada: {chavePrivadaRecuperada}");
    }
}
