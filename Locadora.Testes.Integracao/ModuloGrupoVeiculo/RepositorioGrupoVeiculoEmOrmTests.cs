using Locadora.Dominio.ModuloGrupoVeiculo;
using Locadora.Testes.Integracao.Compartilhado;

namespace Locadora.Testes.Integracao.ModuloGrupoVeiculo;

[TestClass]
[TestCategory("Testes de Integração de grupo veículo")]
public sealed class RepositorioGrupoVeiculoEmOrmTests : TestFixture
{
    [TestMethod]
    public async Task Deve_Cadastrar_GrupoVeiculo_Corretamente()
    {
        // Arrange
        var grupoVeiculo = new GrupoVeiculo("SUV");

        // Act
        await repositorioGrupoVeiculoEmOrm?.CadastrarAsync(grupoVeiculo)!;
        appDbContext?.SaveChanges();

        // Assert
        var registroSelecionado = repositorioGrupoVeiculoEmOrm?.SelecionarRegistroPorIdAsync(grupoVeiculo.Id).Result;

        Assert.AreEqual(grupoVeiculo, registroSelecionado);
    }

    [TestMethod]
    public async Task Deve_Editar_GrupoVeiculo_Corretamente()
    {
        // Arrange
        var grupoVeiculo = new GrupoVeiculo("SUV");

        await repositorioGrupoVeiculoEmOrm?.CadastrarAsync(grupoVeiculo)!;
        appDbContext?.SaveChanges();

        var grupoVeiculoEditado = new GrupoVeiculo("Coupe");

        // Act
        var conseguiuEditar = await repositorioGrupoVeiculoEmOrm?.EditarAsync(grupoVeiculo.Id, grupoVeiculoEditado)!;
        appDbContext?.SaveChanges();

        // Assert
        var registroSelecionado = repositorioGrupoVeiculoEmOrm?.SelecionarRegistroPorIdAsync(grupoVeiculo.Id).Result;
        Assert.AreEqual(grupoVeiculo, registroSelecionado);
        Assert.IsTrue(conseguiuEditar);
    }

    [TestMethod]
    public async Task Deve_Excluir_GrupoVeiculo_Corretamente()
    {
        // Arrange
        var grupoVeiculo = new GrupoVeiculo("SUV");

        await repositorioGrupoVeiculoEmOrm?.CadastrarAsync(grupoVeiculo)!;
        appDbContext?.SaveChanges();

        // Act
        var conseguiuExcluir = await repositorioGrupoVeiculoEmOrm?.ExcluirAsync(grupoVeiculo.Id)!;
        appDbContext?.SaveChanges();

        // Assert
        Assert.IsTrue(conseguiuExcluir);
    }

    [TestMethod]
    public async Task Deve_Selecionar_Todos_GrupoVeiculos_Corretamente()
    {
        // Arrange
        var grupoVeiculo1 = new GrupoVeiculo("SUV");
        var grupoVeiculo2 = new GrupoVeiculo("Hatch");
        var grupoVeiculo3 = new GrupoVeiculo("Sedan");

        await repositorioGrupoVeiculoEmOrm?.CadastrarEntidades(new List<GrupoVeiculo> { grupoVeiculo1, grupoVeiculo2, grupoVeiculo3 })!;
        appDbContext?.SaveChanges();

        // Act
        var gruposVeiculo = await repositorioGrupoVeiculoEmOrm?.SelecionarRegistrosAsync()!;

        // Assert
        Assert.AreEqual(3, gruposVeiculo.Count);
        CollectionAssert.Contains(gruposVeiculo, grupoVeiculo1);
        CollectionAssert.Contains(gruposVeiculo, grupoVeiculo2);
        CollectionAssert.Contains(gruposVeiculo, grupoVeiculo3);
    }

    [TestMethod]
    public async Task Deve_Selecionar_GrupoVeiculo_Com_Quantidade_Especifica_Corretamente()
    {
        // Arrange
        var grupoVeiculo1 = new GrupoVeiculo("SUV");
        var grupoVeiculo2 = new GrupoVeiculo("Hatch");
        var grupoVeiculo3 = new GrupoVeiculo("Sedan");

        await repositorioGrupoVeiculoEmOrm?.CadastrarEntidades(new List<GrupoVeiculo> { grupoVeiculo1, grupoVeiculo2, grupoVeiculo3 })!;
        appDbContext?.SaveChanges();

        // Act
        var gruposVeiculo = await repositorioGrupoVeiculoEmOrm?.SelecionarRegistrosAsync(2)!;

        // Assert
        Assert.AreEqual(2, gruposVeiculo.Count);
    }

    [TestMethod]
    public async Task Deve_Selecionar_GrupoVeiculo_Por_Id_Corretamente()
    {
        // Arrange
        var grupoVeiculo = new GrupoVeiculo("SUV");

        await repositorioGrupoVeiculoEmOrm?.CadastrarAsync(grupoVeiculo)!;
        appDbContext?.SaveChanges();

        // Act
        var grupoVeiculoEncontrado = await repositorioGrupoVeiculoEmOrm?.SelecionarRegistroPorIdAsync(grupoVeiculo.Id)!;

        // Assert
        Assert.AreEqual(grupoVeiculo, grupoVeiculoEncontrado);
    }
}
