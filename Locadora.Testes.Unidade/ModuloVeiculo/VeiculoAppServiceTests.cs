using FluentResults;
using FluentValidation;
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

namespace Locadora.Testes.Unidade.ModuloVeiculo;

[TestClass]
[TestCategory("Testes de unidade de veículo")]
public sealed class VeiculoAppServiceTests
{
    private Mock<ITenantProvider>? tenantProviderMock;
    private Mock<IRepositorioVeiculo>? repositorioVeiculoMock;
    private Mock<IRepositorioAluguel>? repositorioAluguelMock;
    private Mock<IUnitOfWork>? unitOfWorkMock;
    private Mock<ILogger<VeiculoAppService>>? loggerMock;
    private Mock<IValidator<Veiculo>>? validatorMock;

    public VeiculoAppService? veiculoAppService;

    [TestInitialize]
    public void Setup()
    {
        tenantProviderMock = new Mock<ITenantProvider>();
        repositorioVeiculoMock = new Mock<IRepositorioVeiculo>();
        repositorioAluguelMock = new Mock<IRepositorioAluguel>();
        unitOfWorkMock = new Mock<IUnitOfWork>();
        loggerMock = new Mock<ILogger<VeiculoAppService>>();
        validatorMock = new Mock<IValidator<Veiculo>>();

        veiculoAppService = new VeiculoAppService(
            tenantProviderMock.Object,
            repositorioVeiculoMock.Object,
            unitOfWorkMock.Object,
            loggerMock.Object,
            repositorioAluguelMock.Object,
            validatorMock.Object
            );
    }

    [TestMethod]
    public async Task Deve_Cadastrar_Veiculo_Corretamente()
    {
        // Arrange
        var grupoVeiculo = new GrupoVeiculo("SUV");
        var combustivel = new Combustivel("Gasolina", 6);
        var veiculo = new Veiculo(null, grupoVeiculo, "Volkswagen", "Prata", "Phaeton", combustivel, 60, 2008, "BRA5E19", TipoCambioEnum.Automatico, 2000);

        repositorioVeiculoMock?.Setup(r => r.SelecionarRegistrosAsync())
            .ReturnsAsync(new List<Veiculo>() { veiculo });


        validatorMock?.Setup(v => v.ValidateAsync(veiculo, It.IsAny<CancellationToken>()))
.ReturnsAsync(new FluentValidation.Results.ValidationResult());

        // Act
        var resultado = await veiculoAppService!.Cadastrar(veiculo);

        // Assert
        Assert.IsTrue(resultado.IsSuccess);
        repositorioVeiculoMock?.Verify(r => r.CadastrarAsync(veiculo), Times.Once());
        unitOfWorkMock?.Verify(u => u.CommitAsync(), Times.Once);
    }

    [TestMethod]
    public async Task Deve_Editar_Veiculo_Corretamente()
    {
        // Arrange
        var grupoVeiculo = new GrupoVeiculo("SUV");
        var combustivel = new Combustivel("Gasolina", 6);
        var veiculo = new Veiculo(null, grupoVeiculo, "Volkswagen", "Prata", "Phaeton", combustivel, 60, 2008, "BRA5E19", TipoCambioEnum.Automatico, 2000);
        var veiculoEditado = new Veiculo(null, grupoVeiculo, "Volkswagen", "Amarelo", "Nardo", combustivel, 60, 2008, "BRA5E19", TipoCambioEnum.Manual, 22000);


        repositorioVeiculoMock?.Setup(r => r.SelecionarRegistrosAsync())
            .ReturnsAsync(new List<Veiculo>() { veiculo });

        validatorMock?.Setup(v => v.ValidateAsync(veiculoEditado, It.IsAny<CancellationToken>()))
.ReturnsAsync(new FluentValidation.Results.ValidationResult());

        // Act
        var resultado = await veiculoAppService!.Editar(veiculo.Id, veiculoEditado);

        //Assert
        Assert.IsTrue(resultado.IsSuccess);
        repositorioVeiculoMock?.Verify(r => r.EditarAsync(veiculo.Id, veiculoEditado), Times.Once());
        unitOfWorkMock?.Verify(u => u.CommitAsync(), Times.Once);
    }

    [TestMethod]
    public async Task Deve_Falhar_Ao_Editar_Veiculo_Pertence_AAluguel()
    {
        // Arrange
        var grupoVeiculo = new GrupoVeiculo("SUV");
        var combustivel = new Combustivel("Gasolina", 6);
        var veiculo = new Veiculo(null, grupoVeiculo, "Volkswagen", "Prata", "Phaeton", combustivel, 60, 2008, "BRA5E19", TipoCambioEnum.Automatico, 2000);
        var veiculoEditado = new Veiculo(null, grupoVeiculo, "Volkswagen", "Amarelo", "Nardo", combustivel, 60, 2008, "BRA5E19", TipoCambioEnum.Manual, 22000);

        repositorioVeiculoMock?.Setup(r => r.SelecionarRegistrosAsync())
            .ReturnsAsync(new List<Veiculo>() { veiculo });

        var planoCobranca = new Cobranca(grupoVeiculo, Dominio.ModuloCobranca.PlanoCobrancaEnum.Diaria, 100, 100, null, null, null);
        var condutor = new Condutor("Tio Guda", "tioguda@gmail.com", "69 99999-9999", "999.999.999-99", "123456789", DateTime.UtcNow.AddDays(3));

        var aluguel = new Aluguel(condutor, null, planoCobranca, 1000, veiculo, DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(4), null, null, 300, null, false, true, 1000);

        repositorioAluguelMock?.Setup(r => r.SelecionarRegistrosAsync())
            .ReturnsAsync(new List<Aluguel> { aluguel });

        validatorMock?.Setup(v => v.ValidateAsync(veiculoEditado, It.IsAny<CancellationToken>()))
.ReturnsAsync(new FluentValidation.Results.ValidationResult());

        // Act
        var resultado = await veiculoAppService!.Editar(veiculo.Id, veiculoEditado);

        //Assert
        Assert.IsFalse(resultado.IsSuccess);
        repositorioVeiculoMock?.Verify(r => r.EditarAsync(veiculo.Id, veiculoEditado), Times.Never());
        unitOfWorkMock?.Verify(u => u.CommitAsync(), Times.Never);
    }

    [TestMethod]
    public async Task Deve_Excluir_Veiculo_Corretamente()
    {
        // Arrange
        var grupoVeiculo = new GrupoVeiculo("SUV");
        var combustivel = new Combustivel("Gasolina", 6);
        var veiculo = new Veiculo(null, grupoVeiculo, "Volkswagen", "Prata", "Phaeton", combustivel, 60, 2008, "BRA5E19", TipoCambioEnum.Automatico, 2000);

        repositorioVeiculoMock?.Setup(r => r.SelecionarRegistrosAsync())
          .ReturnsAsync(new List<Veiculo>() { veiculo });

        // Act
        var resultado = await veiculoAppService!.Excluir(veiculo.Id);

        // Assert
        Assert.IsTrue(resultado.IsSuccess);
        repositorioVeiculoMock?.Verify(r => r.ExcluirAsync(veiculo.Id), Times.Once());
        unitOfWorkMock?.Verify(u => u.CommitAsync(), Times.Once);
    }

    [TestMethod]
    public async Task Deve_Falhar_A_Excluir_Veiculo_Pertencente_A_Aluguel()
    {
        // Arrange
        var grupoVeiculo = new GrupoVeiculo("SUV");
        var combustivel = new Combustivel("Gasolina", 6);
        var veiculo = new Veiculo(null, grupoVeiculo, "Volkswagen", "Prata", "Phaeton", combustivel, 60, 2008, "BRA5E19", TipoCambioEnum.Automatico, 2000);
        var veiculoEditado = new Veiculo(null, grupoVeiculo, "Volkswagen", "Amarelo", "Nardo", combustivel, 60, 2008, "BRA5E19", TipoCambioEnum.Manual, 22000);

        repositorioVeiculoMock?.Setup(r => r.SelecionarRegistrosAsync())
            .ReturnsAsync(new List<Veiculo>() { veiculo });

        var planoCobranca = new Cobranca(grupoVeiculo, Dominio.ModuloCobranca.PlanoCobrancaEnum.Diaria, 100, 100, null, null, null);
        var condutor = new Condutor("Tio Guda", "tioguda@gmail.com", "69 99999-9999", "999.999.999-99", "123456789", DateTime.UtcNow.AddDays(3));

        var aluguel = new Aluguel(condutor, null, planoCobranca, 1000, veiculo, DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(4), null, null, 300, null, false, true, 1000);

        repositorioAluguelMock?.Setup(r => r.SelecionarRegistrosAsync())
            .ReturnsAsync(new List<Aluguel> { aluguel });

        // Act
        var resultado = await veiculoAppService!.Excluir(veiculo.Id);

        //Assert
        Assert.IsFalse(resultado.IsSuccess);
        repositorioVeiculoMock?.Verify(r => r.ExcluirAsync(veiculo.Id), Times.Never());
        unitOfWorkMock?.Verify(u => u.CommitAsync(), Times.Never);
    }

    [TestMethod]
    public async Task Deve_Filtrar_Veiculos_Por_GrupoVeiculo()
    {
        // Arrange
        var grupoVeiculo = new GrupoVeiculo("SUV");
        var grupoVeiculoEditado = new GrupoVeiculo("Coupé");
        var combustivel = new Combustivel("Gasolina", 6);
        var veiculo = new Veiculo(null, grupoVeiculo, "Volkswagen", "Prata", "Phaeton", combustivel, 60, 2008, "BRA5E19", TipoCambioEnum.Automatico, 2000);
        var veiculoEditado = new Veiculo(null, grupoVeiculoEditado, "Volkswagen", "Amarelo", "Nardo", combustivel, 60, 2008, "BRA5E19", TipoCambioEnum.Manual, 22000);

        repositorioVeiculoMock?.Setup(r => r.FiltrarPorGrupo(grupoVeiculo))
            .ReturnsAsync(new List<Veiculo>() { veiculo });

        // Act
        var resultados = await veiculoAppService!.FiltrarPorGrupo(grupoVeiculo)!;

        // Assert
        Assert.AreEqual(1, resultados.Value.Count);
        CollectionAssert.Contains(resultados.Value, veiculo);
    }
}
