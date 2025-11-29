using DotNet.Testcontainers.Containers;
using FizzWare.NBuilder;
using Locadora.Dominio.ModuloAluguel;
using Locadora.Dominio.ModuloCliente;
using Locadora.Dominio.ModuloCobranca;
using Locadora.Dominio.ModuloCombustivel;
using Locadora.Dominio.ModuloCondutor;
using Locadora.Dominio.ModuloFuncionario;
using Locadora.Dominio.ModuloGrupoVeiculo;
using Locadora.Dominio.ModuloTaxa;
using Locadora.Dominio.ModuloVeiculo;
using Locadora.Infraestrutura.Compartilhado;
using Locadora.Infraestrutura.ModuloAluguel;
using Locadora.Infraestrutura.ModuloCliente;
using Locadora.Infraestrutura.ModuloCobranca;
using Locadora.Infraestrutura.ModuloCombustivel;
using Locadora.Infraestrutura.ModuloCondutor;
using Locadora.Infraestrutura.ModuloFuncionario;
using Locadora.Infraestrutura.ModuloGrupoVeiculo;
using Locadora.Infraestrutura.ModuloTaxa;
using Locadora.Infraestrutura.ModuloVeiculo;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace Locadora.Testes.Integracao.Compartilhado;

[TestClass]
public abstract class TestFixture
{
    protected AppDbContext? appDbContext;

    protected RepositorioGrupoVeiculoEmOrm? repositorioGrupoVeiculoEmOrm;
    protected RepositorioCombustivelEmOrm? repositorioCombustivelEmOrm;
    protected RepositorioVeiculoEmOrm? repositorioVeiculoEmOrm;
    protected RepositorioFuncionarioEmOrm? repositorioFuncionarioEmOrm;
    protected RepositorioClienteEmOrm? repositorioClienteEmOrm;
    protected RepositorioAluguelEmOrm? repositorioAluguelEmOrm;
    protected RepositorioCobrancaEmOrm? repositorioCobrancaEmOrm;
    protected RepositorioCondutorEmOrm? repositorioCondutorEmOrm;
    protected RepositorioTaxaEmOrm? repositorioTaxaEmOrm;

    private static IDatabaseContainer? container;

    [AssemblyInitialize]
    public static async Task Setup(TestContext _)
    {
        container = new PostgreSqlBuilder()
            .WithImage("postgres:16")
            .WithName("locadora-testdb")
            .WithDatabase("LocadoraTestDb")
            .WithUsername("postgres")
            .WithPassword("YourStrongPassword")
            .WithCleanUp(true)
            .Build();

        await InicializarBancoDadosAsync(container);
    }

    [AssemblyCleanup]
    public static async Task Teardown()
    {
        await EncerrarBancoDadosAsync();
    }

    [TestInitialize]
    public void ConfigurarTestes()
    {
        if (container is null)
            throw new ArgumentNullException("O Banco de dados não foi inicializado.");

        appDbContext = AppDbContextFactory.CriarDbContext(container.GetConnectionString());

        ConfigurarTabelas(appDbContext);

        repositorioGrupoVeiculoEmOrm = new RepositorioGrupoVeiculoEmOrm(appDbContext);
        repositorioCombustivelEmOrm = new RepositorioCombustivelEmOrm(appDbContext);
        repositorioTaxaEmOrm = new RepositorioTaxaEmOrm(appDbContext);
        repositorioVeiculoEmOrm = new RepositorioVeiculoEmOrm(appDbContext);
        repositorioCondutorEmOrm = new RepositorioCondutorEmOrm(appDbContext);
        repositorioClienteEmOrm = new RepositorioClienteEmOrm(appDbContext);
        repositorioFuncionarioEmOrm = new RepositorioFuncionarioEmOrm(appDbContext);
        repositorioCobrancaEmOrm = new RepositorioCobrancaEmOrm(appDbContext);
        repositorioAluguelEmOrm = new RepositorioAluguelEmOrm(appDbContext);

        BuilderSetup.SetCreatePersistenceMethod<GrupoVeiculo>(async grupoVeiculo => await repositorioGrupoVeiculoEmOrm.CadastrarAsync(grupoVeiculo));
        BuilderSetup.SetCreatePersistenceMethod<IList<GrupoVeiculo>>(async gruposVeiculos => await repositorioGrupoVeiculoEmOrm.CadastrarEntidades(gruposVeiculos));

        BuilderSetup.SetCreatePersistenceMethod<Combustivel>(async combustivel => await repositorioCombustivelEmOrm.CadastrarAsync(combustivel));
        BuilderSetup.SetCreatePersistenceMethod<IList<Combustivel>>(async combustiveis => await repositorioCombustivelEmOrm.CadastrarEntidades(combustiveis));

        BuilderSetup.SetCreatePersistenceMethod<Taxa>(async taxa => await repositorioTaxaEmOrm.CadastrarAsync(taxa));
        BuilderSetup.SetCreatePersistenceMethod<IList<Taxa>>(async taxas => await repositorioTaxaEmOrm.CadastrarEntidades(taxas));

        BuilderSetup.SetCreatePersistenceMethod<Veiculo>(async veiculo => await repositorioVeiculoEmOrm.CadastrarAsync(veiculo));
        BuilderSetup.SetCreatePersistenceMethod<IList<Veiculo>>(async veiculos => await repositorioVeiculoEmOrm.CadastrarEntidades(veiculos));

        BuilderSetup.SetCreatePersistenceMethod<Condutor>(async condutor => await repositorioCondutorEmOrm.CadastrarAsync(condutor));
        BuilderSetup.SetCreatePersistenceMethod<IList<Condutor>>(async condutores => await repositorioCondutorEmOrm.CadastrarEntidades(condutores));

        BuilderSetup.SetCreatePersistenceMethod<Cliente>(async cliente => await repositorioClienteEmOrm.CadastrarAsync(cliente));
        BuilderSetup.SetCreatePersistenceMethod<IList<Cliente>>(async clientes => await repositorioClienteEmOrm.CadastrarEntidades(clientes));

        BuilderSetup.SetCreatePersistenceMethod<Funcionario>(async funcionario => await repositorioFuncionarioEmOrm.CadastrarAsync(funcionario));
        BuilderSetup.SetCreatePersistenceMethod<IList<Funcionario>>(async funcionarios => await repositorioFuncionarioEmOrm.CadastrarEntidades(funcionarios));

        BuilderSetup.SetCreatePersistenceMethod<Cobranca>(async cobranca => await repositorioCobrancaEmOrm.CadastrarAsync(cobranca));
        BuilderSetup.SetCreatePersistenceMethod<IList<Cobranca>>(async cobrancas => await repositorioCobrancaEmOrm.CadastrarEntidades(cobrancas));

        BuilderSetup.SetCreatePersistenceMethod<Aluguel>(async aluguel => await repositorioAluguelEmOrm.CadastrarAsync(aluguel));
        BuilderSetup.SetCreatePersistenceMethod<IList<Aluguel>>(async alugueis => await repositorioAluguelEmOrm.CadastrarEntidades(alugueis));
    }

    private static void ConfigurarTabelas(AppDbContext dbContext)
    {
        dbContext.Database.EnsureCreated();

        dbContext.aluguel.RemoveRange(dbContext.aluguel);
        dbContext.cobrancas.RemoveRange(dbContext.cobrancas);
        dbContext.funcionarios.RemoveRange(dbContext.funcionarios);
        dbContext.clientes.RemoveRange(dbContext.clientes);
        dbContext.condutores.RemoveRange(dbContext.condutores);
        dbContext.veiculos.RemoveRange(dbContext.veiculos);
        dbContext.taxas.RemoveRange(dbContext.taxas);
        dbContext.combustivels.RemoveRange(dbContext.combustivels);
        dbContext.grupoVeiculos.RemoveRange(dbContext.grupoVeiculos);

        dbContext.SaveChanges();
    }
    private static async Task InicializarBancoDadosAsync(IDatabaseContainer container)
    {
        await container.StartAsync();
    }

    private static async Task EncerrarBancoDadosAsync()
    {
        if (container is null)
            throw new ArgumentNullException("O Banco de dados não foi inicializado.");

        await container.StopAsync();
        await container.DisposeAsync();
    }
}
