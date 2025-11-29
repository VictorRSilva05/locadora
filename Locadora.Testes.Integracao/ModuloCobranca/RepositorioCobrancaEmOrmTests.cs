using Locadora.Dominio.ModuloCobranca;
using Locadora.Dominio.ModuloGrupoVeiculo;
using Locadora.Testes.Integracao.Compartilhado;
using Org.BouncyCastle.Crypto;
using System.Net.WebSockets;

namespace Locadora.Testes.Integracao.ModuloCobranca;

[TestClass]
[TestCategory("Testes de integração de cobrança")]
public sealed class RepositorioCobrancaEmOrmTests : TestFixture
{
    [TestMethod]
    public async Task Deve_Cadastrar_Cobranca_Corretamente()
    {
        // Arrange
        var grupoVeiculo = new GrupoVeiculo("SUV");

        var cobranca = new Cobranca(grupoVeiculo, PlanoCobrancaEnum.Diaria, 50, 50, null, null, null);

        // Act
        await repositorioCobrancaEmOrm?.CadastrarAsync(cobranca)!;
        appDbContext?.SaveChanges();

        // Assert
        var registroSelecionado = repositorioCobrancaEmOrm?.SelecionarRegistroPorIdAsync(cobranca.Id).Result;

        Assert.AreEqual(cobranca, registroSelecionado);
    }

    [TestMethod]
    public async Task Deve_Editar_Cobranca_Corretamente()
    {
        // Arrange
        var grupoVeiculo = new GrupoVeiculo("SUV");
        var grupoVeiculoEditado = new GrupoVeiculo("Sedan");

        var cobranca = new Cobranca(grupoVeiculo, PlanoCobrancaEnum.Livre, null, null, null, null, 3000);

        await repositorioCobrancaEmOrm?.CadastrarAsync(cobranca)!;
        appDbContext?.SaveChanges();

        var cobrancaEditada = new Cobranca(grupoVeiculoEditado, PlanoCobrancaEnum.Diaria, 50, 50, null, null, null);

        // Act
        var conseguiuEditar = await repositorioCobrancaEmOrm?.EditarAsync(cobranca.Id, cobrancaEditada)!;
        appDbContext?.SaveChanges();

        // Assert
        var registroSelecionado = repositorioCobrancaEmOrm?.SelecionarRegistroPorIdAsync(cobranca.Id).Result;
        Assert.AreEqual(cobranca, registroSelecionado);
        Assert.IsTrue(conseguiuEditar);
    }

    [TestMethod]
    public async Task Deve_Excluir_Cobranca_Corretamente()
    {
        // Arrange
        var grupoVeiculo = new GrupoVeiculo("SUV");

        var cobranca = new Cobranca(grupoVeiculo, PlanoCobrancaEnum.Diaria, 50, 50, null, null, null);

        await repositorioCobrancaEmOrm?.CadastrarAsync(cobranca)!;
        appDbContext?.SaveChanges();

        // Act
        var conseguiuExcluir = await repositorioCobrancaEmOrm?.ExcluirAsync(cobranca.Id)!;
        appDbContext?.SaveChanges();

        // Assert
        Assert.IsTrue(conseguiuExcluir);
    }

    [TestMethod]
    public async Task Deve_Selecionar_Todas_Cobrancas_Corretamente()
    {
        // Arrange
        var grupoVeiculo = new GrupoVeiculo("SUV");
        var grupoVeiculoEditado = new GrupoVeiculo("Sedan");

        var cobranca = new Cobranca(grupoVeiculo, PlanoCobrancaEnum.Livre, null, null, null, null, 3000);

        var cobrancaEditada = new Cobranca(grupoVeiculoEditado, PlanoCobrancaEnum.Diaria, 50, 50, null, null, null);

        await repositorioCobrancaEmOrm?.CadastrarEntidades(new List<Cobranca> { cobranca, cobrancaEditada })!;
        appDbContext?.SaveChanges();

        // Act
        var cobrancas = await repositorioCobrancaEmOrm?.SelecionarRegistrosAsync()!;

        // Assert
        Assert.AreEqual(2, cobrancas.Count);
        CollectionAssert.Contains(cobrancas, cobranca);
        CollectionAssert.Contains(cobrancas, cobrancaEditada);
    }

    [TestMethod]
    public async Task Deve_Selecionar_Cobranca_Com_Quantidade_Especifica_Corretamente()
    {
        // Arrange
        var grupoVeiculo = new GrupoVeiculo("SUV");
        var grupoVeiculoEditado = new GrupoVeiculo("Sedan");

        var cobranca = new Cobranca(grupoVeiculo, PlanoCobrancaEnum.Livre, null, null, null, null, 3000);

        var cobrancaEditada = new Cobranca(grupoVeiculoEditado, PlanoCobrancaEnum.Diaria, 50, 50, null, null, null);

        await repositorioCobrancaEmOrm?.CadastrarEntidades(new List<Cobranca> { cobranca, cobrancaEditada })!;
        appDbContext?.SaveChanges();

        // Act
        var cobrancas = await repositorioCobrancaEmOrm?.SelecionarRegistrosAsync(2)!;

        // Assert
        Assert.AreEqual(2, cobrancas.Count);
    }

    [TestMethod]
    public async Task Deve_Selecionar_Cobranca_PorId_Corretamente()
    {
        // Arrange
        var grupoVeiculo = new GrupoVeiculo("SUV");

        var cobranca = new Cobranca(grupoVeiculo, PlanoCobrancaEnum.Diaria, 50, 50, null, null, null);

        // Act
        await repositorioCobrancaEmOrm?.CadastrarAsync(cobranca)!;
        appDbContext?.SaveChanges();

        // Assert
        var registroSelecionado = repositorioCobrancaEmOrm?.SelecionarRegistroPorIdAsync(cobranca.Id).Result;

        Assert.AreEqual(cobranca, registroSelecionado);
    }
}
