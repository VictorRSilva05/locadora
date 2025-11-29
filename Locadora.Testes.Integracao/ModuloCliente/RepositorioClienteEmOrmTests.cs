using Locadora.Dominio.ModuloCliente;
using Locadora.Testes.Integracao.Compartilhado;

namespace Locadora.Testes.Integracao.ModuloCliente;

[TestClass]
[TestCategory("Testes de integração de cliente")]
public sealed class RepositorioClienteEmOrmTests : TestFixture
{

    [TestMethod]
    public async Task Deve_Cadastrar_Cliente_Corretamente()
    {
        // Arrange
        var cliente = new Cliente("Tio Guda", "tioguda@gmail.com", "69 99969-6969", TipoClienteEnum.PessoaFisica, "696.696.696-69", null, "SC", "Ot. Costa", "Santa Catarina", "Sla", "69");

        // Act
        await repositorioClienteEmOrm?.CadastrarAsync(cliente)!;
        appDbContext?.SaveChanges();


        // Assert
        var registroSelecionado = repositorioClienteEmOrm?.SelecionarRegistroPorIdAsync(cliente.Id).Result;

        Assert.AreEqual(cliente, registroSelecionado);
    }

    [TestMethod]
    public async Task Deve_Editar_Cliente_Corretamente()
    {
        // Arrange
        var cliente = new Cliente("Tio Guda", "tioguda@gmail.com", "69 99969-6969", TipoClienteEnum.PessoaFisica, "696.696.696-69", null, "SC", "Ot. Costa", "Santa Catarina", "Sla", "69");

        await repositorioClienteEmOrm?.CadastrarAsync(cliente)!;
        appDbContext?.SaveChanges();

        var clienteEditado = new Cliente("Tahrun", "tahrun@hotmail.com", "67 99969-6969", TipoClienteEnum.PessoaJuridica, null, "XX.XXX.XXX/0001-XX", "PR", "Curitiba", "N sei", "Talvez", "67");

        var conseguiuEditar = await repositorioClienteEmOrm?.EditarAsync(cliente.Id, clienteEditado)!;
        appDbContext?.SaveChanges();

        // Assert
        var registroSelecionado = repositorioClienteEmOrm?.SelecionarRegistroPorIdAsync(cliente.Id).Result;
        Assert.AreEqual(cliente, registroSelecionado);
        Assert.IsTrue(conseguiuEditar);
    }

    [TestMethod]
    public async Task Deve_Excluir_Cliente_Corretamente()
    {
        // Arrange
        var cliente = new Cliente("Tio Guda", "tioguda@gmail.com", "69 99969-6969", TipoClienteEnum.PessoaFisica, "696.696.696-69", null, "SC", "Ot. Costa", "Santa Catarina", "Sla", "69");

        await repositorioClienteEmOrm?.CadastrarAsync(cliente)!;
        appDbContext?.SaveChanges();

        // Act
        var conseguiuExcluir = await repositorioClienteEmOrm?.ExcluirAsync(cliente.Id)!;

        // Assert
        Assert.IsTrue(conseguiuExcluir);
    }

    [TestMethod]
    public async Task Deve_Selecionar_Todos_Clientes_Corretamente()
    {
        // Arrange
        var cliente = new Cliente("Tio Guda", "tioguda@gmail.com", "69 99969-6969", TipoClienteEnum.PessoaFisica, "696.696.696-69", null, "SC", "Ot. Costa", "Santa Catarina", "Sla", "69");

        var clienteEditado = new Cliente("Tahrun", "tahrun@hotmail.com", "67 99969-6969", TipoClienteEnum.PessoaJuridica, null, "XX.XXX.XXX/0001-XX", "PR", "Curitiba", "N sei", "Talvez", "67");

        await repositorioClienteEmOrm?.CadastrarEntidades(new List<Cliente> { cliente, clienteEditado })!;
        appDbContext?.SaveChanges();

        // Act
        var clientes = await repositorioClienteEmOrm?.SelecionarRegistrosAsync()!;

        // Assert
        Assert.AreEqual(2, clientes.Count);
        CollectionAssert.Contains(clientes, cliente);
        CollectionAssert.Contains(clientes, clienteEditado);
    }

    [TestMethod]
    public async Task Deve_Selecionar_Cliente_Com_Quantidade_Especifica_Corretamente()
    {
        // Arrange
        var cliente = new Cliente("Tio Guda", "tioguda@gmail.com", "69 99969-6969", TipoClienteEnum.PessoaFisica, "696.696.696-69", null, "SC", "Ot. Costa", "Santa Catarina", "Sla", "69");

        var clienteEditado = new Cliente("Tahrun", "tahrun@hotmail.com", "67 99969-6969", TipoClienteEnum.PessoaJuridica, null, "XX.XXX.XXX/0001-XX", "PR", "Curitiba", "N sei", "Talvez", "67");

        await repositorioClienteEmOrm?.CadastrarEntidades(new List<Cliente> { cliente, clienteEditado })!;
        appDbContext?.SaveChanges();

        // Act
        var clientes = await repositorioClienteEmOrm?.SelecionarRegistrosAsync(1)!;

        // Assert
        Assert.AreEqual(1, clientes.Count);
    }

    [TestMethod]
    public async Task Deve_Selecionar_Cliente_Por_Id_Corretamente()
    {
        // Arrange
        var cliente = new Cliente("Tio Guda", "tioguda@gmail.com", "69 99969-6969", TipoClienteEnum.PessoaFisica, "696.696.696-69", null, "SC", "Ot. Costa", "Santa Catarina", "Sla", "69");

        // Act
        await repositorioClienteEmOrm?.CadastrarAsync(cliente)!;
        appDbContext?.SaveChanges();

        // Assert
        var registroSelecionado = repositorioClienteEmOrm?.SelecionarRegistroPorIdAsync(cliente.Id).Result;

        Assert.AreEqual(cliente, registroSelecionado);
    }
}
