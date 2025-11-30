using FluentResults;
using Locadora.Aplicacao.ModuloGrupoVeiculo;
using Locadora.Dominio.Autenticacao;
using Locadora.Dominio.Compartilhado;
using Locadora.Dominio.ModuloCobranca;
using Locadora.Dominio.ModuloCombustivel;
using Locadora.Dominio.ModuloGrupoVeiculo;
using Locadora.Dominio.ModuloVeiculo;
using Microsoft.Extensions.Logging;
using Moq;

namespace Locadora.Testes.Unidade.ModuloGrupoVeiculo;

[TestClass]
[TestCategory("Testes de unidade de grupo veículo")]
public sealed class GrupoVeiculoAppServiceTests
{
    private Mock<ITenantProvider>? tenantProviderMock;
    private Mock<IRepositorioGrupoVeiculo>? repositorioGrupoVeiculoMock;
    private Mock<IRepositorioVeiculo>? repositorioVeiculoMock;
    private Mock<IRepositorioCobranca>? repositorioCobrancaMock;
    private Mock<IUnitOfWork>? unitOfWorkMock;
    private Mock<ILogger<GrupoVeiculoAppService>>? loggerMock;

    private GrupoVeiculoAppService? grupoVeiculoAppService;

    [TestInitialize]
    public void Setup()
    {
        tenantProviderMock = new Mock<ITenantProvider>();
        repositorioGrupoVeiculoMock = new Mock<IRepositorioGrupoVeiculo>();
        repositorioVeiculoMock = new Mock<IRepositorioVeiculo>();
        repositorioCobrancaMock = new Mock<IRepositorioCobranca>();
        unitOfWorkMock = new Mock<IUnitOfWork>();
        loggerMock = new Mock<ILogger<GrupoVeiculoAppService>>();

        grupoVeiculoAppService = new GrupoVeiculoAppService(
            tenantProviderMock.Object,
            repositorioGrupoVeiculoMock.Object,
            unitOfWorkMock.Object,
            loggerMock.Object,
            repositorioVeiculoMock.Object,
            repositorioCobrancaMock.Object
            );
    }

    [TestMethod]
    public async Task Deve_Cadastrar_GrupoVeiculo_Com_Sucesso()
    {
        // Arrange
        var grupoVeiculo = new GrupoVeiculo("SUV");

        repositorioGrupoVeiculoMock?.Setup(r => r.SelecionarRegistrosAsync())
            .ReturnsAsync(new List<GrupoVeiculo>());

        // Act
        var resultado = await grupoVeiculoAppService!.Cadastrar(grupoVeiculo);

        // Assert
        Assert.IsTrue(resultado.IsSuccess);
        repositorioGrupoVeiculoMock?.Verify(r => r.CadastrarAsync(grupoVeiculo), Times.Once);
        unitOfWorkMock?.Verify(u => u.CommitAsync(), Times.Once);
    }

    [TestMethod]
    public async Task Deve_Falhar_Ao_Cadastrar_GrupoVeiculo_Com_Mesmo_Nome()
    {
        // Arrange
        var grupoVeiculo = new GrupoVeiculo("SUV");
        var grupoVeiculo2 = new GrupoVeiculo("SUV");

        repositorioGrupoVeiculoMock?.Setup(r => r.SelecionarRegistrosAsync())
            .ReturnsAsync(new List<GrupoVeiculo>() { grupoVeiculo });

        // Act
        var resultado = await grupoVeiculoAppService!.Cadastrar(grupoVeiculo2);

        // Assert
        Assert.IsFalse(resultado.IsSuccess);
        repositorioGrupoVeiculoMock?.Verify(r => r.CadastrarAsync(grupoVeiculo), Times.Never);
        unitOfWorkMock?.Verify(u => u.CommitAsync(), Times.Never);
    }

    [TestMethod]
    public async Task Deve_Editar_GrupoVeiculo_Corretamente()
    {
        // Arrange
        var grupoVeiculo = new GrupoVeiculo("SUV");
        var grupoVeiculoEditado = new GrupoVeiculo("Crossover");

        repositorioGrupoVeiculoMock?.Setup(r => r.SelecionarRegistrosAsync())
            .ReturnsAsync(new List<GrupoVeiculo>() { grupoVeiculo });

        // Act
        var resultado = await grupoVeiculoAppService!.Editar(grupoVeiculo.Id, grupoVeiculoEditado);

        // Assert
        Assert.IsTrue(resultado.IsSuccess);
        repositorioGrupoVeiculoMock?.Verify(r => r.EditarAsync(grupoVeiculo.Id, grupoVeiculoEditado), Times.Once);
        unitOfWorkMock?.Verify(u => u.CommitAsync(), Times.Once);
    }

    [TestMethod]
    public async Task Deve_Falhar_Ao_Editar_GrupoVeiculo_Com_Nome_Ja_Cadastrado()
    {
        // Arrange
        var grupoVeiculoOriginal = new GrupoVeiculo("Crossover");
        var grupoVeiculo = new GrupoVeiculo("SUV");
        var grupoVeiculoEditado = new GrupoVeiculo("Crossover");

        repositorioGrupoVeiculoMock?.Setup(r => r.SelecionarRegistrosAsync())
            .ReturnsAsync(new List<GrupoVeiculo>() { grupoVeiculo, grupoVeiculoOriginal });

        // Act
        var resultado = await grupoVeiculoAppService!.Editar(grupoVeiculo.Id, grupoVeiculoEditado);

        // Assert
        Assert.IsFalse(resultado.IsSuccess);
        repositorioGrupoVeiculoMock?.Verify(r => r.EditarAsync(grupoVeiculo.Id, grupoVeiculoEditado), Times.Never);
        unitOfWorkMock?.Verify(u => u.CommitAsync(), Times.Never);
    }

    [TestMethod]
    public async Task Deve_Excluir_GrupoVeiculo_Com_Sucesso()
    {
        // Arrange
        var grupoVeiculo = new GrupoVeiculo("SUV");

        repositorioGrupoVeiculoMock?.Setup(r => r.SelecionarRegistrosAsync())
            .ReturnsAsync(new List<GrupoVeiculo>() { grupoVeiculo});

        // Act
        var resultado = await grupoVeiculoAppService!.Excluir(grupoVeiculo.Id);

        // Assert
        Assert.IsTrue(resultado.IsSuccess);
        repositorioGrupoVeiculoMock?.Verify(r => r.ExcluirAsync(grupoVeiculo.Id), Times.Once);
        unitOfWorkMock?.Verify(u => u.CommitAsync(), Times.Once);
    }

    [TestMethod]
    public async Task Deve_Falhar_Ao_Excluir_GrupoVeiculo_Com_Veiculos_Cadastrados()
    {
        // Arrange
        var grupoVeiculo = new GrupoVeiculo("SUV");

        repositorioGrupoVeiculoMock?.Setup(r => r.SelecionarRegistrosAsync())
            .ReturnsAsync(new List<GrupoVeiculo>() { grupoVeiculo });

        var combustivel = new Combustivel("Gasolina", 6);
        var veiculo = new Veiculo(null, grupoVeiculo, "Volkswagen", "Prata", "Phaeton", combustivel, 60, 2008, "BRA5E19", TipoCambioEnum.Automatico, 2000);

        repositorioVeiculoMock?.Setup(r => r.SelecionarRegistrosAsync())
            .ReturnsAsync(new List<Veiculo>() { veiculo });

        // Act
        var resultado = await grupoVeiculoAppService!.Excluir(grupoVeiculo.Id);

        // Assert
        Assert.IsFalse(resultado.IsSuccess);
        repositorioGrupoVeiculoMock?.Verify(r => r.ExcluirAsync(grupoVeiculo.Id), Times.Never);
        unitOfWorkMock?.Verify(u => u.CommitAsync(), Times.Never);
    }

    [TestMethod]
    public async Task Deve_Falhar_Ao_Excluir_GrupoVeiculo_Com_Cobrancas_Cadastradas()
    {
        // Arrange
        var grupoVeiculo = new GrupoVeiculo("SUV");

        repositorioGrupoVeiculoMock?.Setup(r => r.SelecionarRegistrosAsync())
            .ReturnsAsync(new List<GrupoVeiculo>() { grupoVeiculo });

        var cobranca = new Cobranca(grupoVeiculo, PlanoCobrancaEnum.Diaria, 100, 100, null, null, null);

        repositorioCobrancaMock?.Setup(r => r.SelecionarRegistrosAsync())
            .ReturnsAsync(new List<Cobranca> { cobranca });
        // Act
        var resultado = await grupoVeiculoAppService!.Excluir(grupoVeiculo.Id);

        // Assert
        Assert.IsFalse(resultado.IsSuccess);
        repositorioGrupoVeiculoMock?.Verify(r => r.ExcluirAsync(grupoVeiculo.Id), Times.Never);
        unitOfWorkMock?.Verify(u => u.CommitAsync(), Times.Never);
    }
}
