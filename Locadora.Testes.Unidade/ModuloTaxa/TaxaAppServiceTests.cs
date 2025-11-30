using Locadora.Aplicacao.ModuloTaxa;
using Locadora.Dominio.Autenticacao;
using Locadora.Dominio.Compartilhado;
using Locadora.Dominio.ModuloAluguel;
using Locadora.Dominio.ModuloCobranca;
using Locadora.Dominio.ModuloCombustivel;
using Locadora.Dominio.ModuloCondutor;
using Locadora.Dominio.ModuloGrupoVeiculo;
using Locadora.Dominio.ModuloTaxa;
using Locadora.Dominio.ModuloVeiculo;
using Microsoft.Extensions.Logging;
using Moq;

namespace Locadora.Testes.Unidade.ModuloTaxa;

[TestClass]
[TestCategory("Testes de unidade de taxas")]
public sealed class TaxaAppServiceTests
{
    private Mock<ITenantProvider>? tenantProviderMock;
    private Mock<IRepositorioTaxa>? repositorioTaxaMock;
    private Mock<IRepositorioAluguel>? repositorioAluguelMock;
    private Mock<IUnitOfWork>? unitOfWorkMock;
    private Mock<ILogger<TaxaAppService>>? loggerMock;

    private TaxaAppService? taxaAppService;

    [TestInitialize]
    public void Setup()
    {
        tenantProviderMock = new Mock<ITenantProvider>();
        repositorioTaxaMock = new Mock<IRepositorioTaxa>();
        repositorioAluguelMock = new Mock<IRepositorioAluguel>();
        unitOfWorkMock = new Mock<IUnitOfWork>();
        loggerMock = new Mock<ILogger<TaxaAppService>>();

        taxaAppService = new TaxaAppService(
            tenantProviderMock.Object,
            repositorioTaxaMock.Object,
            unitOfWorkMock.Object,
            loggerMock.Object,
            repositorioAluguelMock.Object
            );
    }

    [TestMethod]
    public async Task Deve_Cadastrar_Taxa_Corretamente()
    {
        // Arrange
        var taxa = new Taxa("Limpeza", 50, Dominio.ModuloTaxa.PlanoCobrancaEnum.PrecoFixo);

        repositorioTaxaMock?.Setup(r => r.SelecionarRegistrosAsync())
            .ReturnsAsync(new List<Taxa>());

        // Act
        var resultado = await taxaAppService!.Cadastrar(taxa);

        // Assert
        Assert.IsTrue(resultado.IsSuccess);
        repositorioTaxaMock?.Verify(r => r.CadastrarAsync(taxa), Times.Once);
        unitOfWorkMock?.Verify(u => u.CommitAsync(), Times.Once);
    }

    [TestMethod]
    public async Task Deve_Editar_Taxa_Corretamente()
    {
        // Arrange
        var taxa = new Taxa("Limpeza", 50, Dominio.ModuloTaxa.PlanoCobrancaEnum.PrecoFixo);
        var taxaEditada = new Taxa("Valet", 10, Dominio.ModuloTaxa.PlanoCobrancaEnum.CobrancaDiaria);

        repositorioTaxaMock?.Setup(r => r.SelecionarRegistrosAsync())
            .ReturnsAsync(new List<Taxa>() { taxa});

        // Act
        var resultado = await taxaAppService!.Editar(taxa.Id, taxaEditada);

        // Assert
        Assert.IsTrue(resultado.IsSuccess);
        repositorioTaxaMock?.Verify(r => r.EditarAsync(taxa.Id, taxaEditada), Times.Once);
        unitOfWorkMock?.Verify(u => u.CommitAsync(), Times.Once);
    }

    [TestMethod]
    public async Task Deve_Falhar_Ao_Editar_Taxa_Presente_Em_Aluguel()
    {
        // Arrange
        var taxa = new Taxa("Limpeza", 50, Dominio.ModuloTaxa.PlanoCobrancaEnum.PrecoFixo);
        var taxaEditada = new Taxa("Valet", 10, Dominio.ModuloTaxa.PlanoCobrancaEnum.CobrancaDiaria);

        var listaTaxas = new List<Taxa> { taxa };

        repositorioTaxaMock?.Setup(r => r.SelecionarRegistrosAsync())
            .ReturnsAsync(new List<Taxa>() { taxa });

        var grupoVeiculo = new GrupoVeiculo("SUV");
        var combustivel = new Combustivel("Gasolina", 6);
        var veiculo = new Veiculo(null, grupoVeiculo, "Volkswagen", "Prata", "Phaeton", combustivel, 60, 2008, "BRA5E19", TipoCambioEnum.Automatico, 2000);
        var planoCobranca = new Cobranca(grupoVeiculo, Dominio.ModuloCobranca.PlanoCobrancaEnum.Diaria, 100, 100, null, null, null);
        var condutor = new Condutor("Tio Guda", "tioguda@gmail.com", "69 99999-9999","999.999.999-99", "123456789", DateTime.UtcNow.AddDays(3));

        var aluguel = new Aluguel(condutor, null, planoCobranca, 1000, veiculo, DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(4), null, listaTaxas, 300, null, false, true, 1000);

        repositorioAluguelMock?.Setup(r => r.SelecionarRegistrosAsync())
            .ReturnsAsync(new List<Aluguel> { aluguel });

        // Act
        var resultado = await taxaAppService!.Editar(taxa.Id, taxaEditada);

        // Assert
        Assert.IsFalse(resultado.IsSuccess);
        repositorioTaxaMock?.Verify(r => r.EditarAsync(taxa.Id, taxaEditada), Times.Never);
        unitOfWorkMock?.Verify(u => u.CommitAsync(), Times.Never);
    }

    [TestMethod]
    public async Task Deve_Excluir_Taxa_Corretamente()
    {
        // Arrange
        var taxa = new Taxa("Limpeza", 50, Dominio.ModuloTaxa.PlanoCobrancaEnum.PrecoFixo);

        repositorioTaxaMock?.Setup(r => r.SelecionarRegistrosAsync())
            .ReturnsAsync(new List<Taxa>() { taxa });

        // Act
        var resultado = await taxaAppService!.Excluir(taxa.Id);

        // Assert
        Assert.IsTrue(resultado.IsSuccess);
        repositorioTaxaMock?.Verify(r => r.ExcluirAsync(taxa.Id), Times.Once);
        unitOfWorkMock?.Verify(u => u.CommitAsync(), Times.Once);
    }

    [TestMethod]
    public async Task Deve_Falhar_Ao_Excluir_Taxa_Presente_Em_Aluguel()
    {
        // Arrange
        var taxa = new Taxa("Limpeza", 50, Dominio.ModuloTaxa.PlanoCobrancaEnum.PrecoFixo);

        var listaTaxas = new List<Taxa> { taxa };

        repositorioTaxaMock?.Setup(r => r.SelecionarRegistrosAsync())
            .ReturnsAsync(new List<Taxa>() { taxa });

        var grupoVeiculo = new GrupoVeiculo("SUV");
        var combustivel = new Combustivel("Gasolina", 6);
        var veiculo = new Veiculo(null, grupoVeiculo, "Volkswagen", "Prata", "Phaeton", combustivel, 60, 2008, "BRA5E19", TipoCambioEnum.Automatico, 2000);
        var planoCobranca = new Cobranca(grupoVeiculo, Dominio.ModuloCobranca.PlanoCobrancaEnum.Diaria, 100, 100, null, null, null);
        var condutor = new Condutor("Tio Guda", "tioguda@gmail.com", "69 99999-9999", "999.999.999-99", "123456789", DateTime.UtcNow.AddDays(3));

        var aluguel = new Aluguel(condutor, null, planoCobranca, 1000, veiculo, DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(4), null, listaTaxas, 300, null, false, true, 1000);

        repositorioAluguelMock?.Setup(r => r.SelecionarRegistrosAsync())
            .ReturnsAsync(new List<Aluguel> { aluguel });

        // Act
        var resultado = await taxaAppService!.Excluir(taxa.Id);

        // Assert
        Assert.IsFalse(resultado.IsSuccess);
        repositorioTaxaMock?.Verify(r => r.ExcluirAsync(taxa.Id), Times.Never);
        unitOfWorkMock?.Verify(u => u.CommitAsync(), Times.Never);
    }
}
