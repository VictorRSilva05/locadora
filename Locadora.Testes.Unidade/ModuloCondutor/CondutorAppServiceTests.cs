using FluentValidation;
using Locadora.Aplicacao.ModuloCondutor;
using Locadora.Dominio.Autenticacao;
using Locadora.Dominio.Compartilhado;
using Locadora.Dominio.ModuloAluguel;
using Locadora.Dominio.ModuloCobranca;
using Locadora.Dominio.ModuloCombustivel;
using Locadora.Dominio.ModuloCondutor;
using Locadora.Dominio.ModuloGrupoVeiculo;
using Locadora.Dominio.ModuloVeiculo;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locadora.Testes.Unidade.ModuloCondutor;

[TestClass]
[TestCategory("Testes de unidade de condutor")]
public sealed class CondutorAppServiceTests
{
    private Mock<ITenantProvider>? tenantProviderMock;
    private Mock<IRepositorioCondutor>? repositorioCondutorMock;
    private Mock<IRepositorioAluguel>? repositorioAluguelMock;
    private Mock<IUnitOfWork>? unitOfWorkMock;
    private Mock<ILogger<CondutorAppService>>? loggerMock;
    private Mock<IValidator<Condutor>>? validatorMock;

    private CondutorAppService? condutorAppService;

    [TestInitialize]
    public void Setup()
    {
        tenantProviderMock = new Mock<ITenantProvider>();
        repositorioCondutorMock = new Mock<IRepositorioCondutor>();
        repositorioAluguelMock = new Mock<IRepositorioAluguel>();
        unitOfWorkMock = new Mock<IUnitOfWork>();
        loggerMock = new Mock<ILogger<CondutorAppService>>();
        validatorMock = new Mock<IValidator<Condutor>>();

        condutorAppService = new CondutorAppService(
            tenantProviderMock.Object,
            repositorioCondutorMock.Object,
            unitOfWorkMock.Object,
            loggerMock.Object,
            repositorioAluguelMock.Object,
            validatorMock.Object
            );
    }

    [TestMethod]
    public async Task Deve_Cadastrar_Condutor_Com_Sucesso()
    {
        // Arrange 
        var condutor = new Condutor("Tio Guda", "tioguda@gmail.com", "99 99999-9999", "999.999.999-99", "123456789", DateTime.UtcNow.AddYears(1));

        repositorioCondutorMock?.Setup(r => r.SelecionarRegistrosAsync())
            .ReturnsAsync(new List<Condutor>());

        validatorMock?.Setup(v => v.ValidateAsync(condutor, It.IsAny<CancellationToken>()))
    .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        // Act
        var resultado = await condutorAppService!.Cadastrar(condutor);

        // Assert
        Assert.IsTrue(resultado.IsSuccess);
        repositorioCondutorMock?.Verify(r => r.CadastrarAsync(condutor), Times.Once());
        unitOfWorkMock?.Verify(u => u.CommitAsync(), Times.Once());
    }

    [TestMethod]
    public async Task Deve_Falhar_Ao_Cadastrar_Condutor_Com_Mesmo_Cpf()
    {
        // Arrange 
        var condutor = new Condutor("Tio Guda", "tioguda@gmail.com", "99 99999-9999", "999.999.999-99", "123456789", DateTime.UtcNow.AddYears(1));
        var condutor2 = new Condutor("Tio Guda", "xxx@gmail.com", "99 xxx-9999", "999.999.999-99", "xxx", DateTime.UtcNow.AddYears(1));

        repositorioCondutorMock?.Setup(r => r.SelecionarRegistrosAsync())
            .ReturnsAsync(new List<Condutor>() { condutor});

        validatorMock?.Setup(v => v.ValidateAsync(condutor2, It.IsAny<CancellationToken>()))
    .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        // Act
        var resultado = await condutorAppService!.Cadastrar(condutor2);

        // Assert
        Assert.IsFalse(resultado.IsSuccess);
        repositorioCondutorMock?.Verify(r => r.CadastrarAsync(condutor), Times.Never());
        unitOfWorkMock?.Verify(u => u.CommitAsync(), Times.Never());
    }

    [TestMethod]
    public async Task Deve_Falhar_Ao_Cadastrar_Condutor_Com_Mesmo_Email()
    {

        // Arrange 
        var condutor = new Condutor("Tio Guda", "tioguda@gmail.com", "99 99999-9999", "999.999.999-99", "123456789", DateTime.UtcNow.AddYears(1));
        var condutor2 = new Condutor("Tio Guda", "tioguda@gmail.com", "99 xxx-9999", "xxx.999.999-99", "xxx", DateTime.UtcNow.AddYears(1));

        repositorioCondutorMock?.Setup(r => r.SelecionarRegistrosAsync())
            .ReturnsAsync(new List<Condutor>() { condutor });

        validatorMock?.Setup(v => v.ValidateAsync(condutor2, It.IsAny<CancellationToken>()))
    .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        // Act
        var resultado = await condutorAppService!.Cadastrar(condutor2);

        // Assert
        Assert.IsFalse(resultado.IsSuccess);
        repositorioCondutorMock?.Verify(r => r.CadastrarAsync(condutor), Times.Never());
        unitOfWorkMock?.Verify(u => u.CommitAsync(), Times.Never());
    }

    [TestMethod]
    public async Task Deve_Falhar_Ao_Cadastrar_Condutor_Com_A_Mesma_Cnh()
    {
        // Arrange 
        var condutor = new Condutor("Tio Guda", "tioguda@gmail.com", "99 99999-9999", "999.999.999-99", "123456789", DateTime.UtcNow.AddYears(1));
        var condutor2 = new Condutor("Tio Guda", "xxx@gmail.com", "99 xxx-9999", "xxx.999.999-99", "123456789", DateTime.UtcNow.AddYears(1));

        repositorioCondutorMock?.Setup(r => r.SelecionarRegistrosAsync())
            .ReturnsAsync(new List<Condutor>() { condutor });

        validatorMock?.Setup(v => v.ValidateAsync(condutor2, It.IsAny<CancellationToken>()))
    .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        // Act
        var resultado = await condutorAppService!.Cadastrar(condutor2);

        // Assert
        Assert.IsFalse(resultado.IsSuccess);
        repositorioCondutorMock?.Verify(r => r.CadastrarAsync(condutor), Times.Never());
        unitOfWorkMock?.Verify(u => u.CommitAsync(), Times.Never());
    }

    [TestMethod]
    public async Task Deve_Editar_Condutor_Corretamente()
    {
        // Arrange 
        var condutor = new Condutor("Tio Guda", "tioguda@gmail.com", "99 99999-9999", "999.999.999-99", "123456789", DateTime.UtcNow.AddYears(1));
        var condutor2 = new Condutor("Tio Guda", "xxx@gmail.com", "99 xxx-9999", "xxx.999.999-99", "xxx", DateTime.UtcNow.AddYears(1));

        repositorioCondutorMock?.Setup(r => r.SelecionarRegistrosAsync())
            .ReturnsAsync(new List<Condutor>() { condutor });

        validatorMock?.Setup(v => v.ValidateAsync(condutor2, It.IsAny<CancellationToken>()))
    .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        // Act
        var resultado = await condutorAppService!.Editar(condutor.Id,condutor2);

        // Assert
        Assert.IsTrue(resultado.IsSuccess);
        repositorioCondutorMock?.Verify(r => r.EditarAsync(condutor.Id, condutor2), Times.Once());
        unitOfWorkMock?.Verify(u => u.CommitAsync(), Times.Once());
    }

    [TestMethod]
    public async Task Deve_Falhar_Ao_Editar_Condutor_Com_Mesmo_Cpf()
    {
        // Arrange 
        var condutor = new Condutor("Tio Guda", "tioguda@gmail.com", "99 99999-9999", "999.999.999-99", "123456789", DateTime.UtcNow.AddYears(1));
        var condutor2 = new Condutor("Tio Guda", "xxx@gmail.com", "99 xxx-9999", "111.111.111-11", "xxx", DateTime.UtcNow.AddYears(1));
        var condutor3 = new Condutor("Tahrun", "tahrun@hotmail.com", "11 11111-1111", "111.111.111-11", "987654321", DateTime.UtcNow.AddYears(2));

        repositorioCondutorMock?.Setup(r => r.SelecionarRegistrosAsync())
            .ReturnsAsync(new List<Condutor>() { condutor, condutor3 });

        validatorMock?.Setup(v => v.ValidateAsync(condutor2, It.IsAny<CancellationToken>()))
.ReturnsAsync(new FluentValidation.Results.ValidationResult());

        // Act
        var resultado = await condutorAppService!.Editar(condutor.Id, condutor2);

        // Assert
        Assert.IsFalse(resultado.IsSuccess);
        repositorioCondutorMock?.Verify(r => r.EditarAsync(condutor.Id, condutor2), Times.Never());
        unitOfWorkMock?.Verify(u => u.CommitAsync(), Times.Never());
    }

    [TestMethod]
    public async Task Deve_Falhar_Ao_Editar_Condutor_Com_Mesmo_Email()
    {
        // Arrange 
        var condutor = new Condutor("Tio Guda", "tioguda@gmail.com", "99 99999-9999", "999.999.999-99", "123456789", DateTime.UtcNow.AddYears(1));
        var condutor2 = new Condutor("Tio Guda", "tahrun@hotmail.com", "99 xxx-9999", "111.999.999-99", "xxx", DateTime.UtcNow.AddYears(1));
        var condutor3 = new Condutor("Tahrun", "tahrun@hotmail.com", "11 11111-1111", "111.111.111-11", "987654321", DateTime.UtcNow.AddYears(2));

        repositorioCondutorMock?.Setup(r => r.SelecionarRegistrosAsync())
            .ReturnsAsync(new List<Condutor>() { condutor, condutor3 });

        validatorMock?.Setup(v => v.ValidateAsync(condutor2, It.IsAny<CancellationToken>()))
.ReturnsAsync(new FluentValidation.Results.ValidationResult());

        // Act
        var resultado = await condutorAppService!.Editar(condutor.Id, condutor2);

        // Assert
        Assert.IsFalse(resultado.IsSuccess);
        repositorioCondutorMock?.Verify(r => r.EditarAsync(condutor.Id, condutor2), Times.Never());
        unitOfWorkMock?.Verify(u => u.CommitAsync(), Times.Never());
    }

    [TestMethod]
    public async Task Deve_Falhar_Ao_Editar_Condutor_Com_Mesma_Cnh()
    {
        // Arrange 
        var condutor = new Condutor("Tio Guda", "tioguda@gmail.com", "99 99999-9999", "999.999.999-99", "123456789", DateTime.UtcNow.AddYears(1));
        var condutor2 = new Condutor("Tio Guda", "xxx@gmail.com", "99 xxx-9999", "111.999.999-99", "987654321", DateTime.UtcNow.AddYears(1));
        var condutor3 = new Condutor("Tahrun", "tahrun@hotmail.com", "11 11111-1111", "111.111.111-11", "987654321", DateTime.UtcNow.AddYears(2));

        repositorioCondutorMock?.Setup(r => r.SelecionarRegistrosAsync())
            .ReturnsAsync(new List<Condutor>() { condutor, condutor3 });

        validatorMock?.Setup(v => v.ValidateAsync(condutor2, It.IsAny<CancellationToken>()))
.ReturnsAsync(new FluentValidation.Results.ValidationResult());

        // Act
        var resultado = await condutorAppService!.Editar(condutor.Id, condutor2);

        // Assert
        Assert.IsFalse(resultado.IsSuccess);
        repositorioCondutorMock?.Verify(r => r.EditarAsync(condutor.Id, condutor2), Times.Never());
        unitOfWorkMock?.Verify(u => u.CommitAsync(), Times.Never());
    }

    [TestMethod]
    public async Task Deve_Excluir_Condutor_Corretamente()
    {
        // Arrange 
        var condutor = new Condutor("Tio Guda", "tioguda@gmail.com", "99 99999-9999", "999.999.999-99", "123456789", DateTime.UtcNow.AddYears(1));

        repositorioCondutorMock?.Setup(r => r.SelecionarRegistrosAsync())
            .ReturnsAsync(new List<Condutor>() { condutor});

        // Act
        var resultado = await condutorAppService!.Excluir(condutor.Id);

        // Assert
        Assert.IsTrue(resultado.IsSuccess);
        repositorioCondutorMock?.Verify(r => r.ExcluirAsync(condutor.Id), Times.Once());
        unitOfWorkMock?.Verify(u => u.CommitAsync(), Times.Once());
    }

    //[TestMethod]
    //public async Task Deve_Falhar_Ao_Excluir_Condutor_Em_Aluguel()
    //{
    //    // Arrange
    //    var grupoVeiculo = new GrupoVeiculo("SUV");
    //    var combustivel = new Combustivel("Gasolina", 6);
    //    var veiculo = new Veiculo(null, grupoVeiculo, "Volkswagen", "Prata", "Phaeton", combustivel, 60, 2008, "BRA5E19", TipoCambioEnum.Automatico, 2000);
    //    var veiculoEditado = new Veiculo(null, grupoVeiculo, "Volkswagen", "Amarelo", "Nardo", combustivel, 60, 2008, "BRA5E19", TipoCambioEnum.Manual, 22000);
    //    var planoCobranca = new Cobranca(grupoVeiculo, Dominio.ModuloCobranca.PlanoCobrancaEnum.Diaria, 100, 100, null, null, null);
    //    var condutor = new Condutor("Tio Guda", "tioguda@gmail.com", "69 99999-9999", "999.999.999-99", "123456789", DateTime.UtcNow.AddDays(3));

    //    var aluguel = new Aluguel(condutor, null, planoCobranca, 1000, veiculo, DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(4), null, null, 300, null, false, true, 1000);

    //    repositorioAluguelMock?.Setup(r => r.SelecionarRegistrosAsync())
    //        .ReturnsAsync(new List<Aluguel> { aluguel });

    //    repositorioCondutorMock?.Setup(r => r.SelecionarRegistrosAsync())
    //.ReturnsAsync(new List<Condutor>() { condutor });

    //    // Act
    //    var resultado = await condutorAppService!.Excluir(condutor.Id);

    //    //Assert
    //    Assert.IsFalse(resultado.IsSuccess);
    //    repositorioCondutorMock?.Verify(r => r.ExcluirAsync(condutor.Id), Times.Never());
    //    unitOfWorkMock?.Verify(u => u.CommitAsync(), Times.Never());
    //}
}
