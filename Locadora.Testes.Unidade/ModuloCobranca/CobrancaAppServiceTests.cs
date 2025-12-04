using FluentValidation;
using Locadora.Aplicacao.ModuloCobranca;
using Locadora.Aplicacao.ModuloVeiculo;
using Locadora.Dominio.Autenticacao;
using Locadora.Dominio.Compartilhado;
using Locadora.Dominio.ModuloAluguel;
using Locadora.Dominio.ModuloCobranca;
using Locadora.Dominio.ModuloCombustivel;
using Locadora.Dominio.ModuloCondutor;
using Locadora.Dominio.ModuloGrupoVeiculo;
using Locadora.Dominio.ModuloVeiculo;
using Microsoft.Extensions.Logging;
using Moq;

namespace Locadora.Testes.Unidade.ModuloCobranca;

[TestClass]
[TestCategory("Testes de unidade de cobrança")]
public sealed class CobrancaAppServiceTests
{
    private Mock<ITenantProvider>? tenantProviderMock;
    private Mock<IRepositorioCobranca>? repositorioCobrancaMock;
    private Mock<IRepositorioAluguel>? repositorioAluguelMock;
    private Mock<IValidator<Cobranca>>? validatorMock;
    private Mock<IUnitOfWork>? unitOfWorkMock;
    private Mock<ILogger<CobrancaAppService>>? loggerMock;

    private CobrancaAppService? cobrancaAppService;

    [TestInitialize]
    public void Setup()
    {
        tenantProviderMock = new Mock<ITenantProvider>();
        repositorioCobrancaMock = new Mock<IRepositorioCobranca>();
        repositorioAluguelMock = new Mock<IRepositorioAluguel>();
        validatorMock = new Mock<IValidator<Cobranca>>();
        unitOfWorkMock = new Mock<IUnitOfWork>();
        loggerMock = new Mock<ILogger<CobrancaAppService>>();

        cobrancaAppService = new CobrancaAppService(
            tenantProviderMock.Object,
            repositorioCobrancaMock.Object,
            unitOfWorkMock.Object,
            loggerMock.Object,
            repositorioAluguelMock.Object,
            validatorMock.Object
            );
    }

    [TestMethod]
    public async Task Deve_Cadastrar_Cobranca_Com_Sucesso()
    {
        // Arrange
        var grupoVeiculo = new GrupoVeiculo("SUV");
        var cobranca = new Cobranca(grupoVeiculo, PlanoCobrancaEnum.Diaria, 100, 100, null, null, null);

        repositorioCobrancaMock?.Setup(r => r.SelecionarRegistrosAsync())
            .ReturnsAsync(new List<Cobranca>());

        validatorMock?.Setup(v => v.ValidateAsync(cobranca, It.IsAny<CancellationToken>()))
.ReturnsAsync(new FluentValidation.Results.ValidationResult());

        // Act
        var resultado = await cobrancaAppService!.Cadastrar(cobranca);

        // Assert
        Assert.IsTrue(resultado.IsSuccess);
        repositorioCobrancaMock?.Verify(r => r.CadastrarAsync(cobranca), Times.Once());
        unitOfWorkMock?.Verify(u => u.CommitAsync(), Times.Once());
    }

    [TestMethod]
    public  async Task Deve_Editar_Cobranca_Com_Sucesso()
    {
        // Arrange
        var grupoVeiculo = new GrupoVeiculo("SUV");
        var cobranca = new Cobranca(grupoVeiculo, PlanoCobrancaEnum.Diaria, 100, 100, null, null, null);
        var cobrancaEditada = new Cobranca(grupoVeiculo, PlanoCobrancaEnum.Livre, null, null, null, null, 2000);

        repositorioCobrancaMock?.Setup(r => r.SelecionarRegistrosAsync())
            .ReturnsAsync(new List<Cobranca>() { cobranca});

        validatorMock?.Setup(v => v.ValidateAsync(cobrancaEditada, It.IsAny<CancellationToken>()))
.ReturnsAsync(new FluentValidation.Results.ValidationResult());

        // Act
        var resultado = await cobrancaAppService!.Editar(cobranca.Id, cobrancaEditada);

        // Assert
        Assert.IsTrue(resultado.IsSuccess);
        repositorioCobrancaMock?.Verify(r => r.EditarAsync(cobranca.Id, cobrancaEditada), Times.Once());
        unitOfWorkMock?.Verify(u => u.CommitAsync(), Times.Once());
    }

    [TestMethod]
    public async Task Deve_Excluir_Cobranca_Com_Sucesso()
    {
        // Arrange
        var grupoVeiculo = new GrupoVeiculo("SUV");
        var cobranca = new Cobranca(grupoVeiculo, PlanoCobrancaEnum.Diaria, 100, 100, null, null, null);

        repositorioCobrancaMock?.Setup(r => r.SelecionarRegistrosAsync())
            .ReturnsAsync(new List<Cobranca>() { cobranca});

        validatorMock?.Setup(v => v.ValidateAsync(cobranca, It.IsAny<CancellationToken>()))
.ReturnsAsync(new FluentValidation.Results.ValidationResult());

        // Act
        var resultado = await cobrancaAppService!.Excluir(cobranca.Id);

        // Assert
        Assert.IsTrue(resultado.IsSuccess);
        repositorioCobrancaMock?.Verify(r => r.ExcluirAsync(cobranca.Id), Times.Once());
        unitOfWorkMock?.Verify(u => u.CommitAsync(), Times.Once());
    }

   
    //[TestMethod]
    //public async Task Deve_Falhar_Ao_Tentar_Excluir_Cobranca_Em_Aluguel_Ativo()
    //{
    //    // Arrange
    //    var grupoVeiculo = new GrupoVeiculo("SUV");
    //    var combustivel = new Combustivel("Gasolina", 6);
    //    var veiculo = new Veiculo(null, grupoVeiculo, "Volkswagen", "Prata", "Phaeton", combustivel, 60, 2008, "BRA5E19", TipoCambioEnum.Automatico, 2000);
    //    var veiculoEditado = new Veiculo(null, grupoVeiculo, "Volkswagen", "Amarelo", "Nardo", combustivel, 60, 2008, "BRA5E19", TipoCambioEnum.Manual, 22000);
    //    var planoCobranca = new Cobranca(grupoVeiculo, Dominio.ModuloCobranca.PlanoCobrancaEnum.Diaria, 100, 100, null, null, null);

    //    repositorioCobrancaMock?.Setup(r => r.SelecionarRegistrosAsync())
    //        .ReturnsAsync(new List<Cobranca> { planoCobranca });

    //    var condutor = new Condutor("Tio Guda", "tioguda@gmail.com", "69 99999-9999", "999.999.999-99", "123456789", DateTime.UtcNow.AddDays(3));

    //    var aluguel = new Aluguel(condutor, null, planoCobranca, 1000, veiculo, DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(4), null, null, 300, null, false, true, 1000);

    //    repositorioAluguelMock?.Setup(r => r.SelecionarRegistrosAsync())
    //        .ReturnsAsync(new List<Aluguel> { aluguel });

    //    // Act
    //    var resultado = await cobrancaAppService!.Excluir(planoCobranca.Id);

    //    //Assert
    //    Assert.IsFalse(resultado.IsSuccess);
    //    repositorioCobrancaMock?.Verify(r => r.ExcluirAsync(veiculo.Id), Times.Never());
    //    unitOfWorkMock?.Verify(u => u.CommitAsync(), Times.Never);
    //}

}
