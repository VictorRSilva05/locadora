using FluentResults;
using Locadora.Aplicacao.Compartilhado;
using Locadora.Dominio.Autenticacao;
using Locadora.Dominio.Compartilhado;
using Locadora.Dominio.ModuloAluguel;
using Locadora.Dominio.ModuloCobranca;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Locadora.Aplicacao.ModuloCobranca;
public class CobrancaAppService
{
    private readonly ITenantProvider tenantProvider;
    private readonly IRepositorioCobranca repositorioCobranca;
    private readonly IRepositorioAluguel repositorioAluguel;
    private readonly IUnitOfWork unitOfWork;
    private readonly ILogger<CobrancaAppService> logger;

    public CobrancaAppService(
        ITenantProvider tenantProvider,
        IRepositorioCobranca repositorioCobranca,
        IUnitOfWork unitOfWork,
        ILogger<CobrancaAppService> logger,
        IRepositorioAluguel repositorioAluguel)
    {
        this.tenantProvider = tenantProvider;
        this.repositorioCobranca = repositorioCobranca;
        this.unitOfWork = unitOfWork;
        this.logger = logger;
        this.repositorioAluguel = repositorioAluguel;
    }

    public async Task<Result> Cadastrar(Cobranca cobranca)
    {
        if (cobranca.PlanoCobranca == PlanoCobrancaEnum.Diaria && (cobranca.PrecoDiaria == null || cobranca.PrecoKm == null))
            return Result.Fail(
                ResultadosErro.RequisicaoInvalidaErro("Os campos preço diária e preço por Km são necessários no plano diário.")
                );

        if (cobranca.PlanoCobranca == PlanoCobrancaEnum.Controlado && (cobranca.KmDisponiveis == null || cobranca.PrecoDiaria == null || cobranca.PrecoPorKmExtrapolado == null))
            return Result.Fail(
                ResultadosErro.RequisicaoInvalidaErro("Os campos Km disponíveis, preço diária e preço por Km extrapolados são necessários no plano controlado.")
                );

        if (cobranca.PlanoCobranca == PlanoCobrancaEnum.Livre && (cobranca.Taxa == null))
            return Result.Fail(
                ResultadosErro.RequisicaoInvalidaErro("O campo taxa é necessário no plano livre.")
                );
        try
        {
            cobranca.EmpresaId = tenantProvider.TenantId.GetValueOrDefault();

            await repositorioCobranca.CadastrarAsync(cobranca);
            await unitOfWork.CommitAsync();
            return Result.Ok();
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            logger.LogError(
                ex,
                "Ocorreu um erro durante o registro de {@Registro}.",
                cobranca
            );
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }

    public async Task<Result> Editar(Guid id, Cobranca cobranca)
    {
        if (cobranca.PlanoCobranca == PlanoCobrancaEnum.Diaria && cobranca.PrecoDiaria == null || cobranca.PrecoKm == null)
            return Result.Fail(
                ResultadosErro.RequisicaoInvalidaErro("Os campos preço diária e preço por Km são necessários no plano diário.")
                );

        if (cobranca.PlanoCobranca == PlanoCobrancaEnum.Controlado && cobranca.KmDisponiveis == null || cobranca.PrecoDiaria == null || cobranca.PrecoPorKmExtrapolado == null)
            return Result.Fail(
                ResultadosErro.RequisicaoInvalidaErro("Os campos Km disponíveis, preço diária e preço por Km extrapolados são necessários no plano controlado.")
                );

        if (cobranca.PlanoCobranca == PlanoCobrancaEnum.Livre && cobranca.Taxa == null)
            return Result.Fail(
                ResultadosErro.RequisicaoInvalidaErro("O campo taxa é necessário no plano livre.")
                );
        try
        {
            await repositorioCobranca.EditarAsync(id, cobranca);
            await unitOfWork.CommitAsync();
            return Result.Ok();
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            logger.LogError(
                ex,
                "Ocorreu um erro durante a edição de {@Registro}.",
                cobranca
            );
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }

    public async Task<Result> Excluir (Guid id)
    {
        var alugueis = (await repositorioAluguel.SelecionarRegistrosAsync()) ?? new List<Aluguel>();

        if (alugueis.Any(a => a.Status && a.Cobranca.Id == id))
            return Result.Fail(ResultadosErro.ExclusaoBloqueadaErro("Esta cobrança está presente em um aluguel."));

        try
        {
            await repositorioCobranca.ExcluirAsync(id);
            await unitOfWork.CommitAsync();
            return Result.Ok();
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            logger.LogError(
                ex,
                "Ocorreu um erro durante a exclusão do registro com Id {@Id}.",
                id
            );
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }

    public async Task<Result<Cobranca>> SelecionarPorId(Guid id)
    {
        try
        {
            var cobranca = await repositorioCobranca.SelecionarRegistroPorIdAsync(id);
            if (cobranca is null)
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(id));
            return Result.Ok(cobranca);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Ocorreu um erro durante a seleção do registro com Id {@Id}.",
                id
            );
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }

    public async Task<Result<List<Cobranca>>> SelecionarTodos()
    {
        try
        {
            var cobrancas =  await repositorioCobranca.SelecionarRegistrosAsync();
            return Result.Ok(cobrancas);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Ocorreu um erro durante a seleção dos registros de cobrança."
            );
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}
