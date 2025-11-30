using FluentResults;
using Locadora.Aplicacao.Compartilhado;
using Locadora.Dominio.Autenticacao;
using Locadora.Dominio.Compartilhado;
using Locadora.Dominio.ModuloCobranca;
using Locadora.Dominio.ModuloGrupoVeiculo;
using Locadora.Dominio.ModuloVeiculo;
using Microsoft.Extensions.Logging;

namespace Locadora.Aplicacao.ModuloGrupoVeiculo;
public class GrupoVeiculoAppService
{
    private readonly ITenantProvider tenantProvider;
    private readonly IRepositorioGrupoVeiculo repositorioGrupoVeiculo;
    private readonly IRepositorioVeiculo repositorioVeiculo;
    private readonly IRepositorioCobranca repositorioCobranca;
    private readonly IUnitOfWork unitOfWork;
    private readonly ILogger<GrupoVeiculoAppService> logger;

    public GrupoVeiculoAppService(
        ITenantProvider tenantProvider,
        IRepositorioGrupoVeiculo repositorioGrupoVeiculo,
        IUnitOfWork unitOfWork,
        ILogger<GrupoVeiculoAppService> logger,
        IRepositorioVeiculo repositorioVeiculo,
        IRepositorioCobranca repositorioCobranca)
    {
        this.tenantProvider = tenantProvider;
        this.repositorioGrupoVeiculo = repositorioGrupoVeiculo;
        this.unitOfWork = unitOfWork;
        this.logger = logger;
        this.repositorioVeiculo = repositorioVeiculo;
        this.repositorioCobranca = repositorioCobranca;
    }

    public async Task<Result> Cadastrar(GrupoVeiculo grupoVeiculo)
    {
        var registros = await repositorioGrupoVeiculo.SelecionarRegistrosAsync();

        if (registros.Any(i => i.Nome.Equals(grupoVeiculo.Nome)))
            return Result.Fail(ResultadosErro.RegistroDuplicadoErro("Já existe um grupo de veículo com este nome."));

        try
        {
            grupoVeiculo.EmpresaId = tenantProvider.TenantId.GetValueOrDefault();

            await repositorioGrupoVeiculo.CadastrarAsync(grupoVeiculo);

            await unitOfWork.CommitAsync();

            return Result.Ok();
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();

            logger.LogError(
                ex,
                "Ocorreu um erro durante o registro de {@Registro}.",
                grupoVeiculo
            );

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }

    public async Task<Result> Editar(Guid id, GrupoVeiculo grupoVeiculo)
    {
        var registros = await repositorioGrupoVeiculo.SelecionarRegistrosAsync();

        if (registros.Any(i => i.Nome.Equals(grupoVeiculo.Nome)))
            return Result.Fail(ResultadosErro.RegistroDuplicadoErro("Já existe um grupo de veículo com este nome."));

        try
        {
            await repositorioGrupoVeiculo.EditarAsync(id, grupoVeiculo);
            await unitOfWork.CommitAsync();
            return Result.Ok();
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            logger.LogError(
                ex,
                "Ocorreu um erro durante a edição de {@Registro}.",
                grupoVeiculo
            );
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }

    public async Task<Result> Excluir(Guid id)
    {
        var veiculos = (await repositorioVeiculo.SelecionarRegistrosAsync()) ?? new List<Veiculo>();
        var cobrancas = (await repositorioCobranca.SelecionarRegistrosAsync()) ?? new List<Cobranca>();

        if (veiculos.Any(v => v.GrupoVeiculo.Id == id))
            return Result.Fail(ResultadosErro.ExclusaoBloqueadaErro("Há veículos cadastrados nesse grupo."));

        if (cobrancas.Any(c => c.GrupoVeiculo.Id == id))
            return Result.Fail(ResultadosErro.ExclusaoBloqueadaErro("Há planos de cobranças que utilizam este grupo"));

        try
        {
            await repositorioGrupoVeiculo.ExcluirAsync(id);
            await unitOfWork.CommitAsync();
            return Result.Ok();
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            logger.LogError(
                ex,
                "Ocorreu um erro durante a exclusão do registro de id {Id}.",
                id
            );
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }

    public async Task<Result<GrupoVeiculo>> SelecionarPorId(Guid id)
    {
        try
        {
            var grupoVeiculo = await repositorioGrupoVeiculo.SelecionarRegistroPorIdAsync(id);

            if (repositorioGrupoVeiculo is null)
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(id));

            return Result.Ok(grupoVeiculo);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Ocorreu um erro durante a seleção do registro de id {Id}.",
                id
            );
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }

    public async Task<Result<List<GrupoVeiculo>>> SelecionarTodos()
    {
        try
        {
            var registros = await repositorioGrupoVeiculo.SelecionarRegistrosAsync();

            return Result.Ok(registros);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Ocorreu um erro durante a seleção de registros."
                );

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}
