using FluentResults;
using Locadora.Aplicacao.Compartilhado;
using Locadora.Dominio.Autenticacao;
using Locadora.Dominio.Compartilhado;
using Locadora.Dominio.ModuloAluguel;
using Locadora.Dominio.ModuloGrupoVeiculo;
using Locadora.Dominio.ModuloVeiculo;
using Microsoft.Extensions.Logging;

namespace Locadora.Aplicacao.ModuloVeiculo;
public class VeiculoAppService
{
    private readonly ITenantProvider tenantProvider;
    private readonly IRepositorioVeiculo repositorioVeiculo;
    private readonly IRepositorioAluguel repositorioAluguel;
    private readonly IUnitOfWork unitOfWork;
    private readonly ILogger<VeiculoAppService> logger;

    public VeiculoAppService(
        ITenantProvider tenantProvider,
        IRepositorioVeiculo repositorioVeiculo,
        IUnitOfWork unitOfWork,
        ILogger<VeiculoAppService> logger,
        IRepositorioAluguel repositorioAluguel)
    {
        this.tenantProvider = tenantProvider;
        this.repositorioVeiculo = repositorioVeiculo;
        this.unitOfWork = unitOfWork;
        this.logger = logger;
        this.repositorioAluguel = repositorioAluguel;
    }

    public async Task<Result> Cadastrar(Veiculo veiculo)
    {
        try
        {
            veiculo.EmpresaId = tenantProvider.TenantId.GetValueOrDefault();

            await repositorioVeiculo.CadastrarAsync(veiculo);

            await unitOfWork.CommitAsync();
            return Result.Ok();
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            logger.LogError(
                ex,
                "Ocorreu um erro durante o registro de {@Registro}.",
                veiculo
            );
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }

    public async Task<Result> Editar(Guid id, Veiculo veiculo)
    {
        var alugueis = (await repositorioAluguel.SelecionarRegistrosAsync()) ?? new List<Aluguel>();

        if (alugueis.Any(a => a.Status && a.Veiculo.Id == id))
            return Result.Fail(ResultadosErro.RequisicaoInvalidaErro("Este veículo está sendo utilizado em um aluguel."));

        try
        {
            await repositorioVeiculo.EditarAsync(id, veiculo);
            await unitOfWork.CommitAsync();
            return Result.Ok();
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            logger.LogError(
                ex,
                "Ocorreu um erro durante a edição de {@Registro}.",
                veiculo
            );
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }

    public async Task<Result> Excluir(Guid id)
    {
        var alugueis = (await repositorioAluguel.SelecionarRegistrosAsync()) ?? new List<Aluguel>();

        if (alugueis.Any(a => a.Status && a.Veiculo.Id == id))
            return Result.Fail(ResultadosErro.ExclusaoBloqueadaErro("Este veículo está sendo utilizado em um aluguel."));

        try
        {
            await repositorioVeiculo.ExcluirAsync(id);
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

    public async Task<Result<Veiculo>> SelecionarPorId(Guid id)
    {
        try
        {
            var veiculo = await repositorioVeiculo.SelecionarRegistroPorIdAsync(id);

            if (veiculo == null)
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(id));

            return Result.Ok(veiculo);
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

    public async Task<Result<List<Veiculo>>> SelecionarTodos()
    {
        try
        {
            var veiculos = await repositorioVeiculo.SelecionarRegistrosAsync();
            return Result.Ok(veiculos);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Ocorreu um erro durante a seleção dos registros."
            );
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }

    public async Task<Result<List<Veiculo>>> FiltrarPorGrupo(GrupoVeiculo grupoVeiculo)
    {
        try
        {
            var veiculos = await repositorioVeiculo.FiltrarPorGrupo(grupoVeiculo);
            return Result.Ok(veiculos);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Ocorreu um erro durante a seleção dos registros."
            );
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}