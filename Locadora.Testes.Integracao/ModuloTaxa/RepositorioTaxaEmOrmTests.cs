using Locadora.Dominio.ModuloTaxa;
using Locadora.Testes.Integracao.Compartilhado;

namespace Locadora.Testes.Integracao.ModuloTaxa;

[TestClass]
[TestCategory("Testes de integração de taxa")]
public sealed class RepositorioTaxaEmOrmTests : TestFixture
{
    [TestMethod]
    public async Task Deve_Cadastrar_Taxa_Corretamente()
    {
        // Arrange
        var taxa = new Taxa("Limpeza", 30, PlanoCobrancaEnum.PrecoFixo);

        // Act
        await repositorioTaxaEmOrm?.CadastrarAsync(taxa)!;
        appDbContext?.SaveChanges();

        // Assert
        var registroSelecionado = repositorioTaxaEmOrm?.SelecionarRegistroPorIdAsync(taxa.Id).Result;

        Assert.AreEqual(taxa, registroSelecionado);
    }

    [TestMethod]
    public async Task Deve_Editar_Taxa_Corretamente()
    {
        // Arrange
        var taxa = new Taxa("Limpeza", 30, PlanoCobrancaEnum.PrecoFixo);

        await repositorioTaxaEmOrm?.CadastrarAsync(taxa)!;
        appDbContext?.SaveChanges();

        var taxaEditada = new Taxa("GPS", 15, PlanoCobrancaEnum.CobrancaDiaria);

        // Act
        var conseguiuEditar = await repositorioTaxaEmOrm?.EditarAsync(taxa.Id, taxaEditada)!;
        appDbContext?.SaveChanges();

        // Assert
        var registroSelecionado = repositorioTaxaEmOrm?.SelecionarRegistroPorIdAsync(taxa.Id).Result;
        Assert.AreEqual(taxa, registroSelecionado);
        Assert.IsTrue(conseguiuEditar);
    }

    [TestMethod]
    public async Task Deve_Excluir_Taxa_Corretamente()
    {
        // Arrange
        var taxa = new Taxa("Limpeza", 30, PlanoCobrancaEnum.PrecoFixo);

        await repositorioTaxaEmOrm?.CadastrarAsync(taxa)!;
        appDbContext?.SaveChanges();

        // Act
        var conseguiuExcluir = await repositorioTaxaEmOrm?.ExcluirAsync(taxa.Id)!;
        appDbContext?.SaveChanges();

        // Assert
        Assert.IsTrue(conseguiuExcluir);
    }

    [TestMethod]
    public async Task Deve_Selecionar_Todas_Taxas_Corretamente()
    {
        // Arrange
        var taxa = new Taxa("Limpeza", 30, PlanoCobrancaEnum.PrecoFixo);

        var taxaEditada = new Taxa("GPS", 15, PlanoCobrancaEnum.CobrancaDiaria);

        await repositorioTaxaEmOrm?.CadastrarEntidades(new List<Taxa> { taxa, taxaEditada })!;
        appDbContext?.SaveChanges();

        // Act
        var taxas = await repositorioTaxaEmOrm?.SelecionarRegistrosAsync()!;

        // Assert
        Assert.AreEqual(2, taxas.Count);
        CollectionAssert.Contains(taxas, taxaEditada);
        CollectionAssert.Contains(taxas, taxa);
    }

    [TestMethod]
    public async Task Deve_Selecionar_Taxa_Com_Quantidade_Especifica_Corretamente()
    {
        // Arrange
        var taxa = new Taxa("Limpeza", 30, PlanoCobrancaEnum.PrecoFixo);

        var taxaEditada = new Taxa("GPS", 15, PlanoCobrancaEnum.CobrancaDiaria);

        await repositorioTaxaEmOrm?.CadastrarEntidades(new List<Taxa> { taxa, taxaEditada })!;
        appDbContext?.SaveChanges();

        // Act
        var taxas = await repositorioTaxaEmOrm?.SelecionarRegistrosAsync(2)!;

        // Assert
        Assert.AreEqual(2, taxas.Count);
    }

    [TestMethod]
    public async Task Deve_Selecionar_Taxa_PorId_Corretamente()
    {
        // Arrange
        var taxa = new Taxa("Limpeza", 30, PlanoCobrancaEnum.PrecoFixo);

        // Act
        await repositorioTaxaEmOrm?.CadastrarAsync(taxa)!;
        appDbContext?.SaveChanges();

        // Assert
        var registroSelecionado = repositorioTaxaEmOrm?.SelecionarRegistroPorIdAsync(taxa.Id).Result;

        Assert.AreEqual(taxa, registroSelecionado);
    }
}
