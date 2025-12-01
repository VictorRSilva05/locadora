using FluentResults;
using FluentValidation;
using Locadora.Aplicacao.Compartilhado;
using Locadora.Dominio.Autenticacao;
using Locadora.Dominio.Compartilhado;
using Locadora.Dominio.ModuloAluguel;
using Locadora.Dominio.ModuloTaxa;
using Microsoft.Extensions.Logging;

namespace Locadora.Aplicacao.ModuloTaxa;
public class TaxaAppService
{
    private readonly ITenantProvider tenantProvider;
    private readonly IRepositorioTaxa repositorioTaxa;
    private readonly IRepositorioAluguel repositorioAluguel;
    private readonly IValidator<Taxa> validator;
    private readonly IUnitOfWork unitOfWork;
    private readonly ILogger<TaxaAppService> logger;

    public TaxaAppService(
        ITenantProvider tenantProvider,
        IRepositorioTaxa repositorioTaxa,
        IUnitOfWork unitOfWork,
        ILogger<TaxaAppService> logger,
        IRepositorioAluguel repositorioAluguel,
        IValidator<Taxa> validator)
    {
        this.tenantProvider = tenantProvider;
        this.repositorioTaxa = repositorioTaxa;
        this.unitOfWork = unitOfWork;
        this.logger = logger;
        this.repositorioAluguel = repositorioAluguel;
        this.validator = validator;
    }

    public async Task<Result> Cadastrar(Taxa taxa)
    {
        var resultado = await validator.ValidateAsync(taxa);

        if (!resultado.IsValid)
            return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(resultado.Errors.Select(x => x.ErrorMessage)));

        try
        {
            taxa.EmpresaId = tenantProvider.TenantId.GetValueOrDefault();

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
        var resultado = await validator.ValidateAsync(taxa);

        if (!resultado.IsValid)
            return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(resultado.Errors.Select(x => x.ErrorMessage)));

        var alugueis = (await repositorioAluguel.SelecionarRegistrosAsync()) ?? new List<Aluguel>();

        if (alugueis.Any(a => a.Status && a.Taxas!.Any(t => t.Id == id)))
            return Result.Fail(ResultadosErro.RequisicaoInvalidaErro("Esta taxa está sendo utilizada em um aluguel."));

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
        var alugueis = (await repositorioAluguel.SelecionarRegistrosAsync()) ?? new List<Aluguel>();

        if (alugueis.Any(a => a.Status && a.Taxas!.Any(t => t.Id == id)))
            return Result.Fail(ResultadosErro.ExclusaoBloqueadaErro("Esta taxa está sendo utilizada em um aluguel."));

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
            var taxa = await repositorioTaxa.SelecionarRegistroPorIdAsync(id);
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
            var taxas = await repositorioTaxa.SelecionarRegistrosAsync();
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

public class CadastrarTaxaValidator : AbstractValidator<Taxa>
{
    public CadastrarTaxaValidator()
    {
        RuleFor(t => t.Descricao)
            .NotEmpty()
            .WithMessage("O campo {PropertyName} não pode ser vazio");

        RuleFor(t => t.PlanoCobranca)
            .NotEmpty()
            .IsInEnum()
            .WithMessage("O campo plano de cobrança não pode ser vazio");
    }
}
