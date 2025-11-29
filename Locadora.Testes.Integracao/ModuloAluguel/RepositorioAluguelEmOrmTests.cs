using Locadora.Dominio.ModuloAluguel;
using Locadora.Dominio.ModuloCobranca;
using Locadora.Dominio.ModuloCondutor;
using Locadora.Dominio.ModuloGrupoVeiculo;
using Locadora.Testes.Integracao.Compartilhado;

namespace Locadora.Testes.Integracao.ModuloAluguel;

[TestClass]
[TestCategory("Testes de integração de aluguel")]
public sealed class RepositorioAluguelEmOrmTests : TestFixture
{
    [TestMethod]
    public async Task Deve_Cadastrar_Aluguel_Corretamente()
    {
        // Arrange
        var grupoVeiculo = new GrupoVeiculo("SUV");

        var cobranca = new Cobranca(grupoVeiculo, PlanoCobrancaEnum.Diaria, 50, 50, null, null, null);
    }
}
