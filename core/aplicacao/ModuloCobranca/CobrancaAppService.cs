using FluentResults;
using FluentValidation;
using Locadora.Aplicacao.Compartilhado;
using Locadora.Dominio.Autenticacao;
using Locadora.Dominio.Compartilhado;
using Locadora.Dominio.ModuloAluguel;
using Locadora.Dominio.ModuloCobranca;
using Locadora.Dominio.ModuloCombustivel;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Locadora.Aplicacao.ModuloCobranca;
public class CobrancaAppService
{
    private readonly ITenantProvider tenantProvider;
    private readonly IRepositorioCobranca repositorioCobranca;
    private readonly IRepositorioAluguel repositorioAluguel;
    private readonly IValidator<Cobranca> validator;    
    private readonly IUnitOfWork unitOfWork;
    private readonly ILogger<CobrancaAppService> logger;

    public CobrancaAppService(
        ITenantProvider tenantProvider,
        IRepositorioCobranca repositorioCobranca,
        IUnitOfWork unitOfWork,
        ILogger<CobrancaAppService> logger,
        IRepositorioAluguel repositorioAluguel,
        IValidator<Cobranca> validator)
    {
        this.tenantProvider = tenantProvider;
        this.repositorioCobranca = repositorioCobranca;
        this.unitOfWork = unitOfWork;
        this.logger = logger;
        this.repositorioAluguel = repositorioAluguel;
        this.validator = validator;
    }

    public async Task<Result> Cadastrar(Cobranca cobranca)
    {
        var resultado = await validator.ValidateAsync(cobranca);

        if (!resultado.IsValid)
            return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(resultado.Errors.Select(x => x.ErrorMessage)));

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
        var resultado = await validator.ValidateAsync(cobranca);

        if (!resultado.IsValid)
            return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(resultado.Errors.Select(x => x.ErrorMessage)));

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

    public async Task<Result> Excluir(Guid id)
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
            var cobrancas = await repositorioCobranca.SelecionarRegistrosAsync();
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

public class CadastrarCobrancaValidator : AbstractValidator<Cobranca>
{
    public CadastrarCobrancaValidator()
    {
        // ---- PLANO DIÁRIA ----
        When(x => x.PlanoCobranca == PlanoCobrancaEnum.Diaria, () =>
        {
            RuleFor(x => x.PrecoDiaria)
                .NotNull()
                .WithMessage("O campo preço diária é necessário no plano diário.");

            RuleFor(x => x.PrecoKm)
                .NotNull()
                .WithMessage("O campo preço por Km é necessário no plano diário.");
        });

        // ---- PLANO CONTROLADO ----
        When(x => x.PlanoCobranca == PlanoCobrancaEnum.Controlado, () =>
        {
            RuleFor(x => x.KmDisponiveis)
                .NotNull()
                .WithMessage("O campo Km disponíveis é necessário no plano controlado.");

            RuleFor(x => x.PrecoDiaria)
                .NotNull()
                .WithMessage("O campo preço diária é necessário no plano controlado.");

            RuleFor(x => x.PrecoPorKmExtrapolado)
                .NotNull()
                .WithMessage("O campo preço por Km extrapolado é necessário no plano controlado.");
        });

        // ---- PLANO LIVRE ----
        When(x => x.PlanoCobranca == PlanoCobrancaEnum.Livre, () =>
        {
            RuleFor(x => x.Taxa)
                .NotNull()
                .WithMessage("O campo taxa é necessário no plano livre.");
        });
    }
}