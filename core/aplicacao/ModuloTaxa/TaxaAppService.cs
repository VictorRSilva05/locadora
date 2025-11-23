using FluentResults;
using Locadora.Aplicacao.Compartilhado;
using Locadora.Dominio.Compartilhado;
using Locadora.Dominio.ModuloTaxa;
using Microsoft.Extensions.Logging;

namespace Locadora.Aplicacao.ModuloTaxa;
public class TaxaAppService
{
    private readonly IRepositorioTaxa repositorioTaxa;
    private readonly IUnitOfWork unitOfWork;
    private readonly ILogger<TaxaAppService> logger;

    public TaxaAppService(
        IRepositorioTaxa repositorioTaxa,
        IUnitOfWork unitOfWork,
        ILogger<TaxaAppService> logger)
    {
        this.repositorioTaxa = repositorioTaxa;
        this.unitOfWork = unitOfWork;
        this.logger = logger;
    }

    public async Task<Result> Cadastrar(Taxa taxa)
    {
        try
        {
            await repositorioTaxa.CadastrarAsync(taxa);
            await unitOfWork.CommitAsync();
            return Result.Ok();
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            logger.LogError(
                ex,
                "Ocorreu um erro durante o registro de {@Registro}.",
                taxa
            );
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }

    public async Task<Result> Editar(Guid id, Taxa taxa)
    {
        try
        {
            await repositorioTaxa.EditarAsync(id, taxa);
            await unitOfWork.CommitAsync();
            return Result.Ok();
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            logger.LogError(
                ex,
                "Ocorreu um erro durante a edição de {@Registro}.",
                taxa
            );
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }

    public async Task<Result> Excluir(Guid id)
    {
        try
        {
            await repositorioTaxa.ExcluirAsync(id);
            await unitOfWork.CommitAsync();
            return Result.Ok();
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            logger.LogError(
                ex,
                "Ocorreu um erro durante a exclusão de Taxa com Id {Id}.",
                id
            );
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }

    public async Task<Result<Taxa>> SelecionarPorId(Guid id)
    {
        try
        {
            var taxa =  await repositorioTaxa.SelecionarRegistroPorIdAsync(id);
            return Result.Ok(taxa);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Ocorreu um erro durante a seleção de Taxa por Id {Id}.",
                id
            );
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }

    public async Task<Result<List<Taxa>>> SelecionarTodos()
    {
        try
        {
            var taxas =  await repositorioTaxa.SelecionarRegistrosAsync();
            return Result.Ok(taxas);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Ocorreu um erro durante a seleção de todas as Taxas."
            );
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}
