using Castle.Core.Logging;
using Locadora.Aplicacao.ModuloCombustivel;
using Locadora.Dominio.Autenticacao;
using Locadora.Dominio.Compartilhado;
using Locadora.Dominio.ModuloCombustivel;
using Locadora.Dominio.ModuloGrupoVeiculo;
using Locadora.Dominio.ModuloVeiculo;
using Microsoft.Extensions.Logging;
using Moq;

namespace Locadora.Testes.Unidade.ModuloCombustivel;

[TestClass]
[TestCategory("Testes de unidade de combustível")]
public sealed class CombustivelAppServiceTests
{
    private Mock<ITenantProvider>? tenantProviderMock;
    private Mock<IRepositorioCombustivel>? repositorioCombustivelMock;
    private Mock<IRepositorioVeiculo>? repositorioVeiculoMock;
    private Mock<IUnitOfWork>? unitOfWorkMock;
    private Mock<ILogger<CombustivelAppService>>? loggerMock;

    private CombustivelAppService? combustivelAppService;

    [TestInitialize]
    public void Setup()
    {
        tenantProviderMock = new Mock<ITenantProvider>();
        repositorioCombustivelMock = new Mock<IRepositorioCombustivel>();
        repositorioVeiculoMock = new Mock<IRepositorioVeiculo>();
        unitOfWorkMock = new Mock<IUnitOfWork>();
        loggerMock = new Mock<ILogger<CombustivelAppService>>();

        combustivelAppService = new CombustivelAppService(
            tenantProviderMock.Object,
            repositorioCombustivelMock.Object,
            unitOfWorkMock.Object,
            loggerMock.Object,
            repositorioVeiculoMock.Object
            );
    }

    [TestMethod]
    public async Task Deve_Cadastrar_Combustivel_Corretamente()
    {
        // Arrange
        var combustivel = new Combustivel("Gasolina", 6);

        repositorioCombustivelMock?.Setup(r => r.SelecionarRegistrosAsync())
            .ReturnsAsync(new List<Combustivel>());

        // Act
        var resultado = await combustivelAppService!.Cadastrar(combustivel);

        // Assert
        Assert.IsTrue(resultado.IsSuccess);
        repositorioCombustivelMock?.Verify(r => r.CadastrarAsync(combustivel), Times.Once());
        unitOfWorkMock?.Verify(u => u.CommitAsync(), Times.Once);
    }

    [TestMethod]
    public async Task Deve_Falhar_Ao_Cadastrar_Combustivel_Com_Mesmo_Nome()
    {
        // Arrange
        var combustivel = new Combustivel("Gasolina", 6);
        var combustivelRepetido = new Combustivel("Gasolina", 5);

        repositorioCombustivelMock?.Setup(r => r.SelecionarRegistrosAsync())
            .ReturnsAsync(new List<Combustivel>() { combustivel});

        // Act
        var resultado = await combustivelAppService!.Cadastrar(combustivelRepetido);

        // Assert
        Assert.IsFalse(resultado.IsSuccess);
        repositorioCombustivelMock?.Verify(r => r.CadastrarAsync(combustivelRepetido), Times.Never());
        unitOfWorkMock?.Verify(u => u.CommitAsync(), Times.Never);
    }

    [TestMethod]
    public async Task Deve_Editar_Combustivel_Corretamente()
    {
        // Arrange
        var combustivel = new Combustivel("Gasolina", 6);
        var combustivelEditado = new Combustivel("Diesel", 5);

        repositorioCombustivelMock?.Setup(r => r.SelecionarRegistrosAsync())
            .ReturnsAsync(new List<Combustivel>() { combustivel });

        // Act
        var resultado = await combustivelAppService!.Editar(combustivel.Id, combustivelEditado);

        // Assert
        Assert.IsTrue(resultado.IsSuccess);
        repositorioCombustivelMock?.Verify(r => r.EditarAsync(combustivel.Id, combustivelEditado), Times.Once);
        unitOfWorkMock?.Verify(u => u.CommitAsync(), Times.Once);
    }

    [TestMethod]
    public async Task Deve_Falhar_Ao_Editar_Combustivel_Com_Mesmo_Nome()
    {
        // Arrange
        var combustivel = new Combustivel("Gasolina", 6);
        var combustivelEditado = new Combustivel("Gasolina", 5);

        repositorioCombustivelMock?.Setup(r => r.SelecionarRegistrosAsync())
            .ReturnsAsync(new List<Combustivel>() { combustivel });

        // Act
        var resultado = await combustivelAppService!.Editar(combustivel.Id, combustivelEditado);

        // Assert
        Assert.IsFalse(resultado.IsSuccess);
        repositorioCombustivelMock?.Verify(r => r.EditarAsync(combustivel.Id, combustivelEditado), Times.Never);
        unitOfWorkMock?.Verify(u => u.CommitAsync(), Times.Never);
    }

    [TestMethod]
    public async Task Deve_Excluir_Combustivel_Com_Sucesso()
    {
        // Arrange
        var combustivel = new Combustivel("Gasolina", 6);

        repositorioCombustivelMock?.Setup(r => r.SelecionarRegistrosAsync())
            .ReturnsAsync(new List<Combustivel>() { combustivel });

        // Act
        var resultado = await combustivelAppService!.Excluir(combustivel.Id);

        // Assert
        Assert.IsTrue(resultado.IsSuccess);
        repositorioCombustivelMock?.Verify(r => r.ExcluirAsync(combustivel.Id), Times.Once);
        unitOfWorkMock?.Verify(u => u.CommitAsync(), Times.Once);
    }

    [TestMethod]
    public async Task Deve_Falhar_Ao_Tentar_Excluir_Combustivel_Usado_Por_Veiculo()
    {
        // Arrange
        var combustivel = new Combustivel("Gasolina", 6);

        repositorioCombustivelMock?.Setup(r => r.SelecionarRegistrosAsync())
            .ReturnsAsync(new List<Combustivel>() { combustivel });

        var grupoVeiculo = new GrupoVeiculo("Sedan");
        var veiculo = new Veiculo(null, grupoVeiculo, "Volkswagen", "Prata", "Phaeton", combustivel, 60, 2008, "BRA3519", TipoCambioEnum.Automatico, 22200);

        repositorioVeiculoMock?.Setup(r => r.SelecionarRegistrosAsync())
            .ReturnsAsync(new List<Veiculo>() { veiculo });

        // Act
        var resultado = await combustivelAppService!.Excluir(combustivel.Id);

        // Assert
        Assert.IsFalse(resultado.IsSuccess);
        repositorioCombustivelMock?.Verify(r => r.ExcluirAsync(combustivel.Id), Times.Never);
        unitOfWorkMock?.Verify(u => u.CommitAsync(), Times.Never);
    }
}
