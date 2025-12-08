using Locadora.Dominio.Autenticacao;
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

        // Create tenant (empresa) and employee user so FK constraints are satisfied
        var empresa = new User
        {
            UserName = "empresa@test.local",
            Email = "empresa@test.local",
            FullName = "Empresa Teste"
        };

        var userFuncionario = new User
        {
            UserName = "funcionario@test.local",
            Email = "funcionario@test.local",
            FullName = "Funcionario Teste"
        };

        if (appDbContext is null)
            Assert.Fail("appDbContext not initialized for integration test.");

        appDbContext.Users.Add(empresa);
        appDbContext.Users.Add(userFuncionario);
        await appDbContext.SaveChangesAsync();

        var funcionario = new Funcionario("Tio Guda", data, "tioguda@gmail.com", 3000, userFuncionario.Id);
        funcionario.AssociateTenant(empresa.Id);
        funcionario.AssociateUser(userFuncionario);

        // Act
        await repositorioFuncionarioEmOrm?.CadastrarAsync(funcionario)!;
        await appDbContext.SaveChangesAsync();

        // Assert
        var registroSelecionado = await repositorioFuncionarioEmOrm?.SelecionarRegistroPorIdAsync(funcionario.Id);

        Assert.AreEqual(funcionario, registroSelecionado);
    }

    [TestMethod]
    public async Task Deve_Editar_Funcionario_Corretamente()
    {
        // Arrange
        var data = DateTime.UtcNow.AddDays(1);

        // Create tenant (empresa) and employee user so FK constraints are satisfied
        var empresa = new User
        {
            UserName = "empresa@test.local",
            Email = "empresa@test.local",
            FullName = "Empresa Teste"
        };

        var userFuncionario = new User
        {
            UserName = "funcionario@test.local",
            Email = "funcionario@test.local",
            FullName = "Funcionario Teste"
        };

        if (appDbContext is null)
            Assert.Fail("appDbContext not initialized for integration test.");

        appDbContext.Users.Add(empresa);
        appDbContext.Users.Add(userFuncionario);
        await appDbContext.SaveChangesAsync();

        var funcionario = new Funcionario("Tio Guda", data, "tioguda@gmail.com", 3000, userFuncionario.Id);
        funcionario.AssociateTenant(empresa.Id);
        funcionario.AssociateUser(userFuncionario);

        await repositorioFuncionarioEmOrm?.CadastrarAsync(funcionario)!;
        await appDbContext.SaveChangesAsync();

        var funcionarioEditado = new Funcionario("Tahrun", data, "tahrun@hotmail.com", 1200, userFuncionario.Id);

        // Act
        var conseguiuEditar = await repositorioFuncionarioEmOrm?.EditarAsync(funcionario.Id, funcionarioEditado)!;
        appDbContext?.SaveChanges();

        // Assert
        var registroSelecionado = await repositorioFuncionarioEmOrm?.SelecionarRegistroPorIdAsync(funcionario.Id)!;
        Assert.AreEqual(funcionario, registroSelecionado);
        Assert.IsTrue(conseguiuEditar);
    }

    [TestMethod]
    public async Task Deve_Excluir_Funcionario_Corretamente()
    {
        // Arrange
        var data = DateTime.UtcNow.AddDays(1);

        // Create tenant (empresa) and employee user so FK constraints are satisfied
        var empresa = new User
        {
            UserName = "empresa@test.local",
            Email = "empresa@test.local",
            FullName = "Empresa Teste"
        };

        var userFuncionario = new User
        {
            UserName = "funcionario@test.local",
            Email = "funcionario@test.local",
            FullName = "Funcionario Teste"
        };

        if (appDbContext is null)
            Assert.Fail("appDbContext not initialized for integration test.");

        appDbContext.Users.Add(empresa);
        appDbContext.Users.Add(userFuncionario);
        await appDbContext.SaveChangesAsync();

        var funcionario = new Funcionario("Tio Guda", data, "tioguda@gmail.com", 3000, userFuncionario.Id);
        funcionario.AssociateTenant(empresa.Id);
        funcionario.AssociateUser(userFuncionario);

        await repositorioFuncionarioEmOrm?.CadastrarAsync(funcionario)!;
        await appDbContext.SaveChangesAsync();

        // Act
        var conseguiuExcluir = await repositorioFuncionarioEmOrm?.ExcluirAsync(funcionario.Id)!;
        appDbContext?.SaveChanges();

        // Assert
        Assert.IsTrue(conseguiuExcluir);
    }

    [TestMethod]
    public async Task Deve_Selecionar_Todos_Funcionario_Corretamente()
    {
        // Arrange
        var data = DateTime.UtcNow.AddDays(1);

        // Create tenant (empresa) and employee user so FK constraints are satisfied
        var empresa = new User
        {
            UserName = "empresa@test.local",
            Email = "empresa@test.local",
            FullName = "Empresa Teste"
        };

        var userFuncionario = new User
        {
            UserName = "funcionario@test.local",
            Email = "funcionario@test.local",
            FullName = "Funcionario Teste"
        };

        var userFuncionarioEditado = new User
        {
            UserName = "funcionarioEditado@test.local",
            Email = "funcionariEditadoo@test.local",
            FullName = "FuncionarioEditado Teste"
        };

        if (appDbContext is null)
            Assert.Fail("appDbContext not initialized for integration test.");

        appDbContext.Users.Add(empresa);
        appDbContext.Users.Add(userFuncionario);
        appDbContext.Users.Add(userFuncionarioEditado);
        await appDbContext.SaveChangesAsync();

        var funcionario = new Funcionario("Tio Guda", data, "tioguda@gmail.com", 3000, userFuncionario.Id);
        funcionario.AssociateTenant(empresa.Id);
        funcionario.AssociateUser(userFuncionario);

        var funcionarioEditado = new Funcionario("Tahrun", data, "tahrun@hotmail.com", 1200, userFuncionarioEditado.Id);

        funcionarioEditado.AssociateTenant(empresa.Id);

        await repositorioFuncionarioEmOrm?.CadastrarEntidades(new List<Funcionario> { funcionario, funcionarioEditado })!;
        appDbContext?.SaveChanges();

        // Act
        var funcionarios = await repositorioFuncionarioEmOrm?.SelecionarRegistrosAsync()!;

        // Assert
        var registroSelecionado = await repositorioFuncionarioEmOrm?.SelecionarRegistroPorIdAsync(funcionario.Id)!;
        Assert.AreEqual(2, funcionarios.Count);
        CollectionAssert.Contains(funcionarios, funcionario);
        CollectionAssert.Contains(funcionarios, funcionarioEditado);
    }

    [TestMethod]
    public async Task Deve_Selecionar_Funcionario_Com_Quantidade_Especifica_Corretamente()
    {
        // Arrange
        var data = DateTime.UtcNow.AddDays(1);

        // Create tenant (empresa) and employee user so FK constraints are satisfied
        var empresa = new User
        {
            UserName = "empresa@test.local",
            Email = "empresa@test.local",
            FullName = "Empresa Teste"
        };

        var userFuncionario = new User
        {
            UserName = "funcionario@test.local",
            Email = "funcionario@test.local",
            FullName = "Funcionario Teste"
        };

        var userFuncionarioEditado = new User
        {
            UserName = "funcionarioEditado@test.local",
            Email = "funcionariEditadoo@test.local",
            FullName = "FuncionarioEditado Teste"
        };

        if (appDbContext is null)
            Assert.Fail("appDbContext not initialized for integration test.");

        appDbContext.Users.Add(empresa);
        appDbContext.Users.Add(userFuncionario);
        appDbContext.Users.Add(userFuncionarioEditado);
        await appDbContext.SaveChangesAsync();

        var funcionario = new Funcionario("Tio Guda", data, "tioguda@gmail.com", 3000, userFuncionario.Id);
        funcionario.AssociateTenant(empresa.Id);
        funcionario.AssociateUser(userFuncionario);

        var funcionarioEditado = new Funcionario("Tahrun", data, "tahrun@hotmail.com", 1200, userFuncionarioEditado.Id);

        funcionarioEditado.AssociateTenant(empresa.Id);

        await repositorioFuncionarioEmOrm?.CadastrarEntidades(new List<Funcionario> { funcionario, funcionarioEditado })!;
        appDbContext?.SaveChanges();

        // Act
        var funcionarios = await repositorioFuncionarioEmOrm?.SelecionarRegistrosAsync(2)!;

        // Assert
        Assert.AreEqual(2, funcionarios.Count);
    }

    [TestMethod]
    public async Task Deve_Selecionar_Funcionario_PorId_Corretamente()
    {
        // Arrange
        var data = DateTime.UtcNow.AddDays(1);

        // Create tenant (empresa) and employee user so FK constraints are satisfied
        var empresa = new User
        {
            UserName = "empresa@test.local",
            Email = "empresa@test.local",
            FullName = "Empresa Teste"
        };

        var userFuncionario = new User
        {
            UserName = "funcionario@test.local",
            Email = "funcionario@test.local",
            FullName = "Funcionario Teste"
        };

        if (appDbContext is null)
            Assert.Fail("appDbContext not initialized for integration test.");

        appDbContext.Users.Add(empresa);
        appDbContext.Users.Add(userFuncionario);
        await appDbContext.SaveChangesAsync();

        var funcionario = new Funcionario("Tio Guda", data, "tioguda@gmail.com", 3000, userFuncionario.Id);
        funcionario.AssociateTenant(empresa.Id);
        funcionario.AssociateUser(userFuncionario);

        // Act
        await repositorioFuncionarioEmOrm?.CadastrarAsync(funcionario)!;
        await appDbContext.SaveChangesAsync();

        // Assert
        var registroSelecionado = await repositorioFuncionarioEmOrm?.SelecionarRegistroPorIdAsync(funcionario.Id);

        Assert.AreEqual(funcionario, registroSelecionado);
    }
}
