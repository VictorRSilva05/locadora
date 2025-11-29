using Locadora.Dominio.ModuloCombustivel;
using Locadora.Testes.Integracao.Compartilhado;
using System.Net.WebSockets;

namespace Locadora.Testes.Integracao.ModuloCombustivel;

[TestClass]
[TestCategory("Testes de integração de combustível")]
public sealed class RepositorioCombustivelEmOrmTests : TestFixture
{
    [TestMethod]
    public async Task Deve_Cadastrar_Combustivel_Corretamente()
    {
        // Arrange
        var combustivel = new Combustivel("Metanol", 10);

        // Act
        await repositorioCombustivelEmOrm?.CadastrarAsync(combustivel)!;
        appDbContext?.SaveChanges();

        // Assert 
        var registroSelecionado = repositorioCombustivelEmOrm?.SelecionarRegistroPorIdAsync(combustivel.Id).Result;

        Assert.AreEqual(combustivel, registroSelecionado);
    }

    [TestMethod]
    public async Task Deve_Editar_Combustivel_Corretamente()
    {
        // Arrange
        var combustivel = new Combustivel("Gasolina", 6);

        await repositorioCombustivelEmOrm?.CadastrarAsync(combustivel)!;
        appDbContext?.SaveChanges();

        var combustivelEditado = new Combustivel("Diesel", 5);

        // Act
        var conseguiuEditar = await repositorioCombustivelEmOrm?.EditarAsync(combustivel.Id, combustivelEditado)!;
        appDbContext?.SaveChanges();

        // Assert
        var registroSelecionado = repositorioCombustivelEmOrm?.SelecionarRegistroPorIdAsync(combustivel.Id).Result;
        Assert.AreEqual(combustivel, registroSelecionado);
        Assert.IsTrue(conseguiuEditar);
    }

    [TestMethod]
    public async Task Deve_Excluir_Combustivel_Corretamente()
    {
        // Arrange 
        var combustivel = new Combustivel("Etanol", 4);

        await repositorioCombustivelEmOrm?.CadastrarAsync(combustivel)!;
        appDbContext?.SaveChanges();

        // Act
        var conseguiuExcluir = await repositorioCombustivelEmOrm?.ExcluirAsync(combustivel.Id)!;
        appDbContext?.SaveChanges();

        // Assert
        Assert.IsTrue(conseguiuExcluir);
    }

    [TestMethod]
    public async Task Deve_Selecionar_Todos_Combustiveis_Corretamente()
    {
        // Arrange
        var combustivel1 = new Combustivel("Diesel", 6);
        var combustivel2 = new Combustivel("Alcool", 4);
        var combustivel3 = new Combustivel("Gasolina", 6);

        await repositorioCombustivelEmOrm?.CadastrarEntidades(new List<Combustivel> { combustivel1, combustivel2, combustivel3 })!;
        appDbContext?.SaveChanges();

        // Act
        var combustiveis = await repositorioCombustivelEmOrm?.SelecionarRegistrosAsync()!;

        // Assert
        Assert.AreEqual(3, combustiveis.Count);
        CollectionAssert.Contains(combustiveis, combustivel1);
        CollectionAssert.Contains(combustiveis, combustivel2);
        CollectionAssert.Contains(combustiveis, combustivel3);
    }

    [TestMethod]
    public async Task Deve_Selecionar_Combustivel_Com_Quantidade_Especifica_Corretamente()
    {
        // Arrange
        var combustivel1 = new Combustivel("Diesel", 6);
        var combustivel2 = new Combustivel("Alcool", 4);
        var combustivel3 = new Combustivel("Gasolina", 6);

        await repositorioCombustivelEmOrm?.CadastrarEntidades(new List<Combustivel> { combustivel1, combustivel2, combustivel3 })!;
        appDbContext?.SaveChanges();

        // Act
        var combustiveis = await repositorioCombustivelEmOrm?.SelecionarRegistrosAsync(2)!;

        // Assert
        Assert.AreEqual(2, combustiveis.Count);
    }

    [TestMethod]
    public async Task Deve_Selecionar_Combustivel_Por_Id_Corretamente()
    {
        // Arrange
        var combustivel = new Combustivel("Gasolina", 6);

        await repositorioCombustivelEmOrm?.CadastrarAsync(combustivel)!;
        appDbContext?.SaveChanges();

        // Act 
        var combustivelSelecionado = await repositorioCombustivelEmOrm.SelecionarRegistroPorIdAsync(combustivel.Id)!;

        //Assert
        Assert.AreEqual(combustivel, combustivelSelecionado);
    }
}
