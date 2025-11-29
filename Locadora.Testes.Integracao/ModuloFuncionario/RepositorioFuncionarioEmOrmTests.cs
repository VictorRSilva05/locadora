using Locadora.Dominio.ModuloFuncionario;
using Locadora.Testes.Integracao.Compartilhado;

namespace Locadora.Testes.Integracao.ModuloFuncionario;

[TestClass]
[TestCategory("Testes de integração de funcionário")]
public class RepositorioFuncionarioEmOrmTests : TestFixture
{
    [TestMethod]
    public async Task Deve_Cadastrar_Funcionario_Corretamente()
    {
        // Arrange
        var data = DateTime.UtcNow.AddDays(1);

        var funcionario = new Funcionario("Tio Guda", data, "tioguda@gmail.com", 3000, Guid.NewGuid());

        // Act
        await repositorioFuncionarioEmOrm?.CadastrarAsync(funcionario)!;
        appDbContext?.SaveChanges();

        // Assert
        var registroSelecionado = repositorioFuncionarioEmOrm?.SelecionarRegistroPorIdAsync(funcionario.Id).Result;

        Assert.AreEqual(funcionario, registroSelecionado);
    }
}
