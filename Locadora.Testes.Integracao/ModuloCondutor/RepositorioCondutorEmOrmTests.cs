using Locadora.Dominio.ModuloCondutor;
using Locadora.Testes.Integracao.Compartilhado;

namespace Locadora.Testes.Integracao.ModuloCondutor;

[TestClass]
[TestCategory("Testes de integração de condutor")]
public sealed class RepositorioCondutorEmOrmTests : TestFixture
{
    [TestMethod]
    public async Task Deve_Cadastrar_Condutor_Corretamente()
    {
        // Arrange
        var data = DateTime.UtcNow.AddDays(2);

        var condutor = new Condutor("Tio Guda", "tioguda@gmail.com", "69 99969-6767", "696.696.696-69", "123456789", data);

        // Act
        await repositorioCondutorEmOrm?.CadastrarAsync(condutor)!;
        appDbContext?.SaveChanges();

        // Assert
        var registroSelecionado = repositorioCondutorEmOrm?.SelecionarRegistroPorIdAsync(condutor.Id).Result;

        Assert.AreEqual(condutor, registroSelecionado);
    }

    [TestMethod]
    public async Task Deve_Editar_Condutor_Corretamente()
    {
        // Arrange
        var data = DateTime.UtcNow.AddDays(2);

        var condutor = new Condutor("Tio Guda", "tioguda@gmail.com", "69 99969-6767", "696.696.696-69", "123456789", data);

        await repositorioCondutorEmOrm?.CadastrarAsync(condutor)!;
        appDbContext?.SaveChanges();

        var dataEditada = DateTime.UtcNow.AddDays(5);

        var condutorEditado = new Condutor("Tahrun", "tahrun@hotmail.com", "67 99967-67676", "676.676.676-67", "987654321", dataEditada);

        // Act
        var conseguiuEditar = await repositorioCondutorEmOrm?.EditarAsync(condutor.Id, condutorEditado)!;
        appDbContext?.SaveChanges();

        // Assert
        var registroSelecionado = repositorioCondutorEmOrm?.SelecionarRegistroPorIdAsync(condutor.Id).Result;
        Assert.AreEqual(condutor, registroSelecionado);
        Assert.IsTrue(conseguiuEditar);
    }

    [TestMethod]
    public async Task Deve_Excluir_Condutor_Corretamente()
    {
        // Arrange
        var data = DateTime.UtcNow.AddDays(2);

        var condutor = new Condutor("Tio Guda", "tioguda@gmail.com", "69 99969-6767", "696.696.696-69", "123456789", data);

        await repositorioCondutorEmOrm?.CadastrarAsync(condutor)!;
        appDbContext?.SaveChanges();

        // Act
        var conseguiuExcluir = await repositorioCondutorEmOrm?.ExcluirAsync(condutor.Id)!;
        appDbContext?.SaveChanges();

        // Assert
        Assert.IsTrue(conseguiuExcluir);
    }

    [TestMethod]
    public async Task Deve_Selecionar_Todos_Condutores_Corretamente()
    {
        //Arrange
        var data = DateTime.UtcNow.AddDays(2);

        var condutor = new Condutor("Tio Guda", "tioguda@gmail.com", "69 99969-6767", "696.696.696-69", "123456789", data);

        var dataEditada = DateTime.UtcNow.AddDays(5);

        var condutorEditado = new Condutor("Tahrun", "tahrun@hotmail.com", "67 99967-67676", "676.676.676-67", "987654321", dataEditada);

        await repositorioCondutorEmOrm?.CadastrarEntidades(new List<Condutor> { condutor, condutorEditado })!;
        appDbContext?.SaveChanges();

        // Act
        var condutores = await repositorioCondutorEmOrm?.SelecionarRegistrosAsync()!;

        //Assert
        Assert.AreEqual(2, condutores.Count);
        CollectionAssert.Contains(condutores, condutor);
        CollectionAssert.Contains(condutores, condutorEditado);
    }

    [TestMethod]
    public async Task Deve_Selecionar_Condutor_Com_Quantidade_Especifica_Corretamente()
    {
        //Arrange
        var data = DateTime.UtcNow.AddDays(2);

        var condutor = new Condutor("Tio Guda", "tioguda@gmail.com", "69 99969-6767", "696.696.696-69", "123456789", data);

        var dataEditada = DateTime.UtcNow.AddDays(5);

        var condutorEditado = new Condutor("Tahrun", "tahrun@hotmail.com", "67 99967-67676", "676.676.676-67", "987654321", dataEditada);

        await repositorioCondutorEmOrm?.CadastrarEntidades(new List<Condutor> { condutor, condutorEditado })!;
        appDbContext?.SaveChanges();

        // Act
        var condutores = await repositorioCondutorEmOrm?.SelecionarRegistrosAsync(1)!;

        // Assert
        Assert.AreEqual(1, condutores.Count);
    }

    [TestMethod]
    public async Task Deve_Selecionar_Condutor_Por_Id_Corretamente()
    {
        // Arrange
        var data = DateTime.UtcNow.AddDays(2);

        var condutor = new Condutor("Tio Guda", "tioguda@gmail.com", "69 99969-6767", "696.696.696-69", "123456789", data);

        await repositorioCondutorEmOrm?.CadastrarAsync(condutor)!;
        appDbContext?.SaveChanges();

        // Act
        var condutorEncontrado = await repositorioCondutorEmOrm?.SelecionarRegistroPorIdAsync(condutor.Id)!;

        // Assert
        Assert.AreEqual(condutor, condutorEncontrado);
    }
}
