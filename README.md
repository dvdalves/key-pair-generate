# GeradorParChaves

Fluxo Completo da Criptografia
Gerar Par de Chaves RSA:

O método GeradorParChave.GerarParChaves() gera uma chave pública e uma chave privada usando o algoritmo RSA. A chave pública é usada para criptografar dados, enquanto a chave privada é usada para descriptografá-los.
Armazenar e Buscar Par de Chaves:

A chave privada é criptografada usando AES e armazenada no arquivo chavePrivada.bson via Armazenamento.ArmazenarChavePrivada().
A chave privada pode ser recuperada e descriptografada via Armazenamento.BuscarChavePrivada().
Gerar Segredo:

O segredo (uma chave simétrica) é gerado usando o método GeradorSegredo.GerarSegredo(). Esse segredo é usado para criptografar objetos e arquivos.
Armazenar e Buscar Segredo:

O segredo é criptografado com AES e armazenado no arquivo segredo.bson via Armazenamento.ArmazenarSegredo().
O segredo pode ser recuperado e descriptografado via Armazenamento.BuscarSegredo().
Criptografar e Descriptografar Segredo:

O segredo pode ser criptografado com a chave pública RSA usando GeradorSegredo.CriptografarSegredo() e descriptografado com a chave privada RSA usando GeradorSegredo.DescriptografarSegredo().
Criptografar e Descriptografar Objetos:

Objetos (dados binários) são criptografados usando o segredo descriptografado com AES via GeradorSegredo.CriptografarObjeto() e podem ser descriptografados usando GeradorSegredo.DescriptografarObjeto().
Criptografar e Descriptografar Arquivos:

Arquivos são criptografados e descriptografados da mesma maneira que objetos, utilizando GeradorSegredo.CriptografarArquivo() e GeradorSegredo.DescriptografarArquivo().
