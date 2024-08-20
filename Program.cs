using GeradorParChaves.Services;

class Program
{
    static void Main(string[] args)
    {
        var geradorParChaves = new GeradorParChave();
        var armazenamentoChave = new ArmazenamentoChave();
        var servicoAuditoria = new ServicoAuditoria();

        var (chavePublica, chavePrivada) = geradorParChaves.GerarParChaves();

        armazenamentoChave.ArmazenarChavePrivada(chavePrivada, "chavePrivada.enc");
        servicoAuditoria.AuditarGeracaoChave("admin");

        Console.WriteLine("Chave pública gerada: " + chavePublica);
    }
}
