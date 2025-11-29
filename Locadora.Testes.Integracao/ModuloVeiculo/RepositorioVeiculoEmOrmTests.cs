using Locadora.Dominio.ModuloCombustivel;
using Locadora.Dominio.ModuloGrupoVeiculo;
using Locadora.Dominio.ModuloVeiculo;
using Locadora.Testes.Integracao.Compartilhado;

namespace Locadora.Testes.Integracao.ModuloVeiculo;

[TestClass]
[TestCategory("Testes de Integração de veículo")]
public sealed class RepositorioVeiculoEmOrmTests : TestFixture
{
    [TestMethod]
    public async Task Deve_Cadastrar_Veiculo_Corretamente()
    {
        // Arrange
        var grupoVeiculo = new GrupoVeiculo("SUV");

        await repositorioGrupoVeiculoEmOrm?.CadastrarAsync(grupoVeiculo)!;
        appDbContext?.SaveChanges();

        var combustivel = new Combustivel("Diesel", 5);

        await repositorioCombustivelEmOrm?.CadastrarAsync(combustivel)!;
        appDbContext?.SaveChanges();

        // Act
        var veiculo = new Veiculo(null, grupoVeiculo, "Audi", "Q7", "Prata", combustivel, 70, 2007, "BRA3S14", TipoCambioEnum.Automatico, 200);

        await repositorioVeiculoEmOrm?.CadastrarAsync(veiculo)!;
        appDbContext?.SaveChanges();

        // Assert
        var registroSelecionado = repositorioVeiculoEmOrm?.SelecionarRegistroPorIdAsync(veiculo.Id).Result;

        Assert.AreEqual(veiculo, registroSelecionado);
    }

    [TestMethod]
    public async Task Deve_Editar_Veiculo_Corretamente()
    {
        // Arrange
        var grupoVeiculo = new GrupoVeiculo("SUV");

        await repositorioGrupoVeiculoEmOrm?.CadastrarAsync(grupoVeiculo)!;
        appDbContext?.SaveChanges();

        var combustivel = new Combustivel("Diesel", 5);

        await repositorioCombustivelEmOrm?.CadastrarAsync(combustivel)!;
        appDbContext?.SaveChanges();

        var grupoVeiculoEditado = new GrupoVeiculo("Sedan");

        await repositorioGrupoVeiculoEmOrm?.CadastrarAsync(grupoVeiculoEditado)!;
        appDbContext?.SaveChanges();

        var combustivelEditado = new Combustivel("Gasolina", 5);

        await repositorioCombustivelEmOrm?.CadastrarAsync(combustivelEditado)!;
        appDbContext?.SaveChanges();

        var veiculo = new Veiculo(null, grupoVeiculo, "Audi", "Q7", "Prata", combustivel, 70, 2007, "BRA3S14", TipoCambioEnum.Automatico, 200);

        await repositorioVeiculoEmOrm?.CadastrarAsync(veiculo)!;
        appDbContext?.SaveChanges();

        var veiculoEditado = new Veiculo(null, grupoVeiculoEditado, "Renault","Twingo", "Verde", combustivelEditado, 40, 1995, "FRA3S69", TipoCambioEnum.Manual, 2200);

        // Act
        var conseguiuEditar = await repositorioVeiculoEmOrm?.EditarAsync(veiculo.Id,veiculoEditado)!;
        appDbContext?.SaveChanges();

        // Assert
        var registroSelecionado = repositorioVeiculoEmOrm?.SelecionarRegistroPorIdAsync(veiculo.Id).Result;
        Assert.AreEqual(veiculo, registroSelecionado);
        Assert.IsTrue(conseguiuEditar);
    }

    [TestMethod]
    public async Task Deve_Excluir_Veiculo_Corretamente()
    {
        // Arrange
        var grupoVeiculo = new GrupoVeiculo("SUV");

        await repositorioGrupoVeiculoEmOrm?.CadastrarAsync(grupoVeiculo)!;
        appDbContext?.SaveChanges();

        var combustivel = new Combustivel("Diesel", 5);

        await repositorioCombustivelEmOrm?.CadastrarAsync(combustivel)!;
        appDbContext?.SaveChanges();

        var veiculo = new Veiculo(null, grupoVeiculo, "Audi", "Q7", "Prata", combustivel, 70, 2007, "BRA3S14", TipoCambioEnum.Automatico, 200);

        await repositorioVeiculoEmOrm?.CadastrarAsync(veiculo)!;
        appDbContext?.SaveChanges();

        // Act
        var conseguiuExcluir = await repositorioVeiculoEmOrm?.ExcluirAsync(veiculo.Id)!;
        appDbContext?.SaveChanges();

        // Assert
        Assert.IsTrue(conseguiuExcluir);
    }

    [TestMethod]
    public async Task Deve_Selecionar_Todos_Veiculos_Corretamente()
    {
        // Arrange
        var grupoVeiculo = new GrupoVeiculo("SUV");

        await repositorioGrupoVeiculoEmOrm?.CadastrarAsync(grupoVeiculo)!;
        appDbContext?.SaveChanges();

        var combustivel = new Combustivel("Diesel", 5);

        await repositorioCombustivelEmOrm?.CadastrarAsync(combustivel)!;
        appDbContext?.SaveChanges();

        var grupoVeiculoEditado = new GrupoVeiculo("Sedan");

        await repositorioGrupoVeiculoEmOrm?.CadastrarAsync(grupoVeiculoEditado)!;
        appDbContext?.SaveChanges();

        var combustivelEditado = new Combustivel("Gasolina", 5);

        await repositorioCombustivelEmOrm?.CadastrarAsync(combustivelEditado)!;
        appDbContext?.SaveChanges();

        var veiculo = new Veiculo(null, grupoVeiculo, "Audi", "Q7", "Prata", combustivel, 70, 2007, "BRA3S14", TipoCambioEnum.Automatico, 200);

        var veiculoEditado = new Veiculo(null, grupoVeiculoEditado, "Renault", "Twingo", "Q7", combustivelEditado, 40, 1995, "FRA3S69", TipoCambioEnum.Manual, 2200);

        await repositorioVeiculoEmOrm?.CadastrarEntidades(new List<Veiculo> { veiculo, veiculoEditado })!;
        appDbContext?.SaveChanges();

        // Act
        var veiculos = await repositorioVeiculoEmOrm?.SelecionarRegistrosAsync()!;

        // Assert
        Assert.AreEqual(2, veiculos.Count);
        CollectionAssert.Contains(veiculos, veiculo);
        CollectionAssert.Contains(veiculos, veiculoEditado);
    }

    [TestMethod]
    public async Task Deve_Selecionar_Veiculos_Com_Quantidade_Especifica_Corretamente()
    {
        // Arrange
        var grupoVeiculo = new GrupoVeiculo("SUV");

        await repositorioGrupoVeiculoEmOrm?.CadastrarAsync(grupoVeiculo)!;
        appDbContext?.SaveChanges();

        var combustivel = new Combustivel("Diesel", 5);

        await repositorioCombustivelEmOrm?.CadastrarAsync(combustivel)!;
        appDbContext?.SaveChanges();

        var grupoVeiculoEditado = new GrupoVeiculo("Sedan");

        await repositorioGrupoVeiculoEmOrm?.CadastrarAsync(grupoVeiculoEditado)!;
        appDbContext?.SaveChanges();

        var combustivelEditado = new Combustivel("Gasolina", 5);

        await repositorioCombustivelEmOrm?.CadastrarAsync(combustivelEditado)!;
        appDbContext?.SaveChanges();

        var veiculo = new Veiculo(null, grupoVeiculo, "Audi", "Q7", "Prata", combustivel, 70, 2007, "BRA3S14", TipoCambioEnum.Automatico, 200);

        var veiculoEditado = new Veiculo(null, grupoVeiculoEditado, "Renault", "Twingo", "Q7", combustivelEditado, 40, 1995, "FRA3S69", TipoCambioEnum.Manual, 2200);

        await repositorioVeiculoEmOrm?.CadastrarEntidades(new List<Veiculo> { veiculo, veiculoEditado })!;
        appDbContext?.SaveChanges();

        // Act
        var veiculoSelecionado = await repositorioVeiculoEmOrm?.SelecionarRegistrosAsync(1)!;

        // Assert
        Assert.AreEqual(1, veiculoSelecionado.Count);
    }

    [TestMethod]
    public async Task Deve_Selecionar_Veiculo_Por_Id_Corretamente()
    {
        // Arrange
        var grupoVeiculo = new GrupoVeiculo("SUV");

        await repositorioGrupoVeiculoEmOrm?.CadastrarAsync(grupoVeiculo)!;
        appDbContext?.SaveChanges();

        var combustivel = new Combustivel("Diesel", 5);

        await repositorioCombustivelEmOrm?.CadastrarAsync(combustivel)!;
        appDbContext?.SaveChanges();

        var veiculo = new Veiculo(null, grupoVeiculo, "Audi", "Q7", "Prata", combustivel, 70, 2007, "BRA3S14", TipoCambioEnum.Automatico, 200);

        await repositorioVeiculoEmOrm?.CadastrarAsync(veiculo)!;
        appDbContext?.SaveChanges();

        // Act
        var veiculoSelecionado = await repositorioVeiculoEmOrm?.SelecionarRegistroPorIdAsync(veiculo.Id)!;

        // Assert
        Assert.AreEqual(veiculo, veiculoSelecionado);
    }
}
