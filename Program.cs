using BenchmarkDotNet.Running;
using GeradorParChaves.Benchmarks;

class Program
{
    static void Main(string[] args)
    {
        // Para rodar o benchmark, execute o programa com o argumento "benchmark":
        // dotnet run -c Release -- benchmark 
        var summary = BenchmarkRunner.Run<CriptografiaBenchmark>();


        //var geradorParChaves = new GeradorParChave();
        //var chaveSecreta = ArmazenamentoChave.GerarChaveSecreta();
        //var iv = ArmazenamentoChave.GerarIV();

        //var armazenamentoChave = new ArmazenamentoChave(chaveSecreta, iv);
        //var servicoAuditoria = new ServicoAuditoria();

        //var (chavePublica, chavePrivada) = geradorParChaves.GerarParChaves();

        //Guid fornecedorId = Guid.NewGuid();
        //armazenamentoChave.ArmazenarChavePrivada(chavePrivada, fornecedorId);
        //servicoAuditoria.AuditarGeracaoChave("admin");

        //Console.WriteLine("Chave pública gerada: " + chavePublica);

        //var (fornecedorIdRecuperado, chavePrivadaRecuperada) = armazenamentoChave.BuscarChavePrivada();
        //Console.WriteLine($"Fornecedor: {fornecedorIdRecuperado}, Chave privada recuperada: {chavePrivadaRecuperada}");
    }
}
