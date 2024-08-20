# GeradorParChaves

## Visão Geral

O **GeradorParChaves** é uma aplicação em .NET para gerar pares de chaves RSA, armazenar a chave privada com criptografia AES e associá-la a um fornecedor identificado por um `Guid`.

## Funcionalidades

- Geração de pares de chaves RSA.
- Criptografia e armazenamento seguro da chave privada em BSON.
- Associação da chave privada a um `Guid` de fornecedor.
- Auditoria da geração de chaves.

## Estrutura do Projeto

- `Program.cs`: Ponto de entrada da aplicação.
- `Services/ArmazenamentoChave.cs`: Criptografa e armazena a chave privada.
- `Services/GeradorParChave.cs`: Gera o par de chaves RSA.
- `Services/ServicoAuditoria.cs`: Registra auditoria da geração de chaves.
