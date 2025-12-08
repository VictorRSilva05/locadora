using Locadora.Dominio.ModuloAluguel;
using Locadora.Dominio.ModuloCobranca;
using Locadora.Dominio.ModuloCombustivel;
using Locadora.Dominio.ModuloCondutor;
using Locadora.Dominio.ModuloGrupoVeiculo;
using Locadora.Dominio.ModuloVeiculo;
using Locadora.Testes.Integracao.Compartilhado;

namespace Locadora.Testes.Integracao.ModuloAluguel;

[TestClass]
[TestCategory("Testes de integração de aluguel")]
public sealed class RepositorioAluguelEmOrmTests : TestFixture
{
    //[TestMethod]
    //public async Task Deve_Cadastrar_Aluguel_Corretamente()
    //{
    //    // Arrange
    //    var grupoVeiculo = new GrupoVeiculo("SUV");

    //    var cobranca = new Cobranca(grupoVeiculo, PlanoCobrancaEnum.Diaria, 50, 50, null, null, null);

    //    var data = DateTime.UtcNow.AddDays(2);

    //    var condutor = new Condutor("Tio Guda", "tioguda@gmail.com", "69 99969-6767", "696.696.696-69", "123456789", data);

    //    var combustivel = new Combustivel("Diesel", 5);

    //    var veiculo = new Veiculo(null, grupoVeiculo, "Audi", "Q7", "Prata", combustivel, 70, 2007, "BRA3S14", TipoCambioEnum.Automatico, 200);

    //    var dataSaida = DateTime.UtcNow.AddDays(1);

    //    var dataRetornoPrevista = DateTime.UtcNow.AddDays(3);

    //    var aluguel = new Aluguel(condutor, null, cobranca, 1000, veiculo, dataSaida, dataRetornoPrevista, null, null, veiculo.Kilometragem, null, null, true, 0);

    //    // Act
    //    await repositorioAluguelEmOrm?.CadastrarAsync(aluguel)!;
    //    appDbContext?.SaveChanges();

    //    // Assert
    //    var registroSelecionado = repositorioAluguelEmOrm?.SelecionarRegistroPorIdAsync(aluguel.Id).Result;

    //    Assert.AreEqual(aluguel, registroSelecionado);
    //}

    //[TestMethod]
    //public async Task Deve_Editar_Aluguel_Corretamente()
    //{
    //    // Arrange
    //    var grupoVeiculo = new GrupoVeiculo("SUV");

    //    var grupoVeiculoEditado = new GrupoVeiculo("Coupe");

    //    var cobranca = new Cobranca(grupoVeiculo, PlanoCobrancaEnum.Diaria, 50, 50, null, null, null);

    //    var data = DateTime.UtcNow.AddDays(2);

    //    var condutor = new Condutor("Tio Guda", "tioguda@gmail.com", "69 99969-6767", "696.696.696-69", "123456789", data);

    //    var condutorEditado = new Condutor("Tahrun", "tahrun@hotmail.com", "67 99967-67676", "676.676.676-67", "987654321", data);

    //    var combustivel = new Combustivel("Diesel", 5);

    //    var combustivelEditado = new Combustivel("Gasolina", 5);

    //    var veiculo = new Veiculo(null, grupoVeiculo, "Audi", "Q7", "Prata", combustivel, 70, 2007, "BRA3S14", TipoCambioEnum.Automatico, 200);

    //    var veiculoEditado = new Veiculo(null, grupoVeiculoEditado, "Renault", "Twingo", "Verde", combustivelEditado, 40, 1995, "FRA3S69", TipoCambioEnum.Manual, 2200);

    //    var dataSaida = DateTime.UtcNow.AddDays(1);

    //    var dataRetornoPrevista = DateTime.UtcNow.AddDays(3);

    //    var aluguel = new Aluguel(condutor, null, cobranca, 1000, veiculo, dataSaida, dataRetornoPrevista, null, null, veiculo.Kilometragem, null, null, true, 0);

    //    var aluguelEditado = new Aluguel(condutorEditado, null, cobranca, 200, veiculoEditado, dataSaida, dataRetornoPrevista, null, null, veiculo.Kilometragem, null, null, true, 0);

    //    await repositorioAluguelEmOrm?.CadastrarAsync(aluguel)!;
    //    appDbContext?.SaveChanges();

    //    // Act
    //    var conseguiuEditar = await repositorioAluguelEmOrm?.EditarAsync(aluguel.Id, aluguelEditado)!;

    //    // Assert
    //    var registroSelecionado = repositorioAluguelEmOrm?.SelecionarRegistroPorIdAsync(aluguel.Id).Result;
    //    Assert.AreEqual(aluguel, registroSelecionado);
    //    Assert.IsTrue(conseguiuEditar);
    //}

    //[TestMethod]
    //public async Task Deve_Excluir_Aluguel_Corretamente()
    //{
    //    // Arrange
    //    var grupoVeiculo = new GrupoVeiculo("SUV");

    //    var cobranca = new Cobranca(grupoVeiculo, PlanoCobrancaEnum.Diaria, 50, 50, null, null, null);

    //    var data = DateTime.UtcNow.AddDays(2);

    //    var condutor = new Condutor("Tio Guda", "tioguda@gmail.com", "69 99969-6767", "696.696.696-69", "123456789", data);

    //    var combustivel = new Combustivel("Diesel", 5);

    //    var veiculo = new Veiculo(null, grupoVeiculo, "Audi", "Q7", "Prata", combustivel, 70, 2007, "BRA3S14", TipoCambioEnum.Automatico, 200);

    //    var dataSaida = DateTime.UtcNow.AddDays(1);

    //    var dataRetornoPrevista = DateTime.UtcNow.AddDays(3);

    //    var aluguel = new Aluguel(condutor, null, cobranca, 1000, veiculo, dataSaida, dataRetornoPrevista, null, null, veiculo.Kilometragem, null, null, true, 0);

    //    await repositorioAluguelEmOrm?.CadastrarAsync(aluguel)!;
    //    appDbContext?.SaveChanges();

    //    // Act
    //    var conseguiuExcluir = await repositorioAluguelEmOrm?.ExcluirAsync(aluguel.Id)!;
    //    appDbContext?.SaveChanges();

    //    // Assert
    //    Assert.IsTrue(conseguiuExcluir);
    //}

    //[TestMethod]
    //public async Task Deve_Selecionar_Aluguel_Com_Quantidade_Especifica_Corretamente()
    //{
    //    // Arrange
    //    var grupoVeiculo = new GrupoVeiculo("SUV");

    //    var grupoVeiculoEditado = new GrupoVeiculo("Coupe");

    //    var cobranca = new Cobranca(grupoVeiculo, PlanoCobrancaEnum.Diaria, 50, 50, null, null, null);

    //    var data = DateTime.UtcNow.AddDays(2);

    //    var condutor = new Condutor("Tio Guda", "tioguda@gmail.com", "69 99969-6767", "696.696.696-69", "123456789", data);

    //    var condutorEditado = new Condutor("Tahrun", "tahrun@hotmail.com", "67 99967-67676", "676.676.676-67", "987654321", data);

    //    var combustivel = new Combustivel("Diesel", 5);

    //    var combustivelEditado = new Combustivel("Gasolina", 5);

    //    var veiculo = new Veiculo(null, grupoVeiculo, "Audi", "Q7", "Prata", combustivel, 70, 2007, "BRA3S14", TipoCambioEnum.Automatico, 200);

    //    var veiculoEditado = new Veiculo(null, grupoVeiculoEditado, "Renault", "Twingo", "Verde", combustivelEditado, 40, 1995, "FRA3S69", TipoCambioEnum.Manual, 2200);

    //    var dataSaida = DateTime.UtcNow.AddDays(1);

    //    var dataRetornoPrevista = DateTime.UtcNow.AddDays(3);

    //    var aluguel = new Aluguel(condutor, null, cobranca, 1000, veiculo, dataSaida, dataRetornoPrevista, null, null, veiculo.Kilometragem, null, null, true, 0);

    //    var aluguelEditado = new Aluguel(condutorEditado, null, cobranca, 200, veiculoEditado, dataSaida, dataRetornoPrevista, null, null, veiculo.Kilometragem, null, null, true, 0);

    //    await repositorioAluguelEmOrm?.CadastrarEntidades(new List<Aluguel> { aluguel, aluguelEditado })!;
    //    appDbContext?.SaveChanges();

    //    // Act
    //    var alugueis = await repositorioAluguelEmOrm?.SelecionarRegistrosAsync(2)!;

    //    // Assert
    //    Assert.AreEqual(2, alugueis.Count);
    //}

    //[TestMethod]
    //public async Task Deve_Selecionar_Todos_Aluguel_Corretamente()
    //{
    //    // Arrange
    //    var grupoVeiculo = new GrupoVeiculo("SUV");

    //    var grupoVeiculoEditado = new GrupoVeiculo("Coupe");

    //    var cobranca = new Cobranca(grupoVeiculo, PlanoCobrancaEnum.Diaria, 50, 50, null, null, null);

    //    var data = DateTime.UtcNow.AddDays(2);

    //    var condutor = new Condutor("Tio Guda", "tioguda@gmail.com", "69 99969-6767", "696.696.696-69", "123456789", data);

    //    var condutorEditado = new Condutor("Tahrun", "tahrun@hotmail.com", "67 99967-67676", "676.676.676-67", "987654321", data);

    //    var combustivel = new Combustivel("Diesel", 5);

    //    var combustivelEditado = new Combustivel("Gasolina", 5);

    //    var veiculo = new Veiculo(null, grupoVeiculo, "Audi", "Q7", "Prata", combustivel, 70, 2007, "BRA3S14", TipoCambioEnum.Automatico, 200);

    //    var veiculoEditado = new Veiculo(null, grupoVeiculoEditado, "Renault", "Twingo", "Verde", combustivelEditado, 40, 1995, "FRA3S69", TipoCambioEnum.Manual, 2200);

    //    var dataSaida = DateTime.UtcNow.AddDays(1);

    //    var dataRetornoPrevista = DateTime.UtcNow.AddDays(3);

    //    var aluguel = new Aluguel(condutor, null, cobranca, 1000, veiculo, dataSaida, dataRetornoPrevista, null, null, veiculo.Kilometragem, null, null, true, 0);

    //    var aluguelEditado = new Aluguel(condutorEditado, null, cobranca, 200, veiculoEditado, dataSaida, dataRetornoPrevista, null, null, veiculo.Kilometragem, null, null, true, 0);

    //    await repositorioAluguelEmOrm?.CadastrarEntidades(new List<Aluguel> { aluguel, aluguelEditado })!;
    //    appDbContext?.SaveChanges();

    //    // Act
    //    var alugueis = await repositorioAluguelEmOrm?.SelecionarRegistrosAsync()!;

    //    // Assert
    //    Assert.AreEqual(2, alugueis.Count);
    //}

    //[TestMethod]
    //public async Task Deve_Selecionar_Aluguel_PorId_Corretamente()
    //{
    //    // Arrange
    //    var grupoVeiculo = new GrupoVeiculo("SUV");

    //    var cobranca = new Cobranca(grupoVeiculo, PlanoCobrancaEnum.Diaria, 50, 50, null, null, null);

    //    var data = DateTime.UtcNow.AddDays(2);

    //    var condutor = new Condutor("Tio Guda", "tioguda@gmail.com", "69 99969-6767", "696.696.696-69", "123456789", data);

    //    var combustivel = new Combustivel("Diesel", 5);

    //    var veiculo = new Veiculo(null, grupoVeiculo, "Audi", "Q7", "Prata", combustivel, 70, 2007, "BRA3S14", TipoCambioEnum.Automatico, 200);

    //    var dataSaida = DateTime.UtcNow.AddDays(1);

    //    var dataRetornoPrevista = DateTime.UtcNow.AddDays(3);

    //    var aluguel = new Aluguel(condutor, null, cobranca, 1000, veiculo, dataSaida, dataRetornoPrevista, null, null, veiculo.Kilometragem, null, null, true, 0);

    //    // Act
    //    await repositorioAluguelEmOrm?.CadastrarAsync(aluguel)!;
    //    appDbContext?.SaveChanges();

    //    // Assert
    //    var registroSelecionado = repositorioAluguelEmOrm?.SelecionarRegistroPorIdAsync(aluguel.Id).Result;

    //    Assert.AreEqual(aluguel, registroSelecionado);
    //}
}
