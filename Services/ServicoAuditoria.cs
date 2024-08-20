namespace GeradorParChaves.Services;

public class ServicoAuditoria
{
    public void AuditarGeracaoChave(string usuario)
    {
        string entradaLog = $"{DateTime.UtcNow}: {usuario} gerou um novo par de chaves RSA.";
        File.AppendAllText("auditoria.log", entradaLog + Environment.NewLine);
    }
}