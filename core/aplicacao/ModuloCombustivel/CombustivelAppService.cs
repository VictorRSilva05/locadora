using FluentResults;
using FluentValidation;
using Locadora.Aplicacao.Compartilhado;
using Locadora.Dominio.Autenticacao;
using Locadora.Dominio.Compartilhado;
using Locadora.Dominio.ModuloCombustivel;
using Locadora.Dominio.ModuloGrupoVeiculo;
using Locadora.Dominio.ModuloVeiculo;
using Microsoft.Extensions.Logging;

namespace Locadora.Aplicacao.ModuloCombustivel;
public class CombustivelAppService
{
    private readonly ITenantProvider tenantProvider;
    private readonly IRepositorioCombustivel repositorioCombustivel;
    private readonly IRepositorioVeiculo repositorioVeiculo;
    private readonly IValidator<Combustivel> validator;
    private readonly IUnitOfWork unitOfWork;
    private readonly ILogger<CombustivelAppService> logger;

    public CombustivelAppService(
        ITenantProvider tenantProvider,
        IRepositorioCombustivel repositorioCombustivel,
        IUnitOfWork unitOfWork,
        ILogger<CombustivelAppService> logger,
        IRepositorioVeiculo repositorioVeiculo,
        IValidator<Combustivel> validator)
    {
        this.tenantProvider = tenantProvider;
        this.repositorioCombustivel = repositorioCombustivel;
        this.unitOfWork = unitOfWork;
        this.logger = logger;
        this.repositorioVeiculo = repositorioVeiculo;
        this.validator = validator;
    }

    public async Task<Result> Cadastrar(Combustivel combustivel)
    {
        var resultado = await validator.ValidateAsync(combustivel);

        if (!resultado.IsValid)
            return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(resultado.Errors.Select(x => x.ErrorMessage)));

        var registros = await repositorioCombustivel.SelecionarRegistrosAsync();

        if (registros.Any(i => i.Nome.Equals(combustivel.Nome)))
            return Result.Fail(ResultadosErro.RegistroDuplicadoErro("Já existe um combustível com este nome."));

        try
        {
            combustivel.EmpresaId = tenantProvider.TenantId.GetValueOrDefault();

            await repositorioCombustivel.CadastrarAsync(combustivel);
            await unitOfWork.CommitAsync();
            return Result.Ok();
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            logger.LogError(
                ex,
                "Ocorreu um erro durante o registro de {@Registro}.",
                combustivel
            );
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }

    public async Task<Result> Editar(Guid id, Combustivel combustivel)
    {
        var resultado = await validator.ValidateAsync(combustivel);

        if (!resultado.IsValid)
            return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(resultado.Errors.Select(x => x.ErrorMessage)));

        var registros = await repositorioCombustivel.SelecionarRegistrosAsync();

        if (registros.Any(i =>  i.Id != id && i.Nome.Equals(combustivel.Nome)))
            return Result.Fail(ResultadosErro.RegistroDuplicadoErro("Já existe um combustível com este nome."));
        try
        {
            await repositorioCombustivel.EditarAsync(id, combustivel);
            await unitOfWork.CommitAsync();
            return Result.Ok();
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            logger.LogError(
                ex,
                "Ocorreu um erro durante a edição de {@Registro}.",
                combustivel
            );
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }

    public async Task<Result> Excluir(Guid id)
    {
        var veiculos = (await repositorioVeiculo.SelecionarRegistrosAsync()) ?? new List<Veiculo>();

        if (veiculos.Any(v => v.Combustivel.Id == id))
            return Result.Fail(ResultadosErro.ExclusaoBloqueadaErro("Há veículos cadastrados com esse combustível."));

        try
        {
            await repositorioCombustivel.ExcluirAsync(id);
            await unitOfWork.CommitAsync();
            return Result.Ok();
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            logger.LogError(
                ex,
                "Ocorreu um erro durante a exclusão do registro de Id {@Id}.",
                id
            );
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }

    public async Task<Result<Combustivel>> SelecionarPorId(Guid id)
    {
        try
        {
            var combustivel = await repositorioCombustivel.SelecionarRegistroPorIdAsync(id);

            if (repositorioCombustivel == null)
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(id));

            return Result.Ok(combustivel);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Ocorreu um erro durante a seleção do registro de Id {@Id}.",
                id
            );
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }

    public async Task<Result<List<Combustivel>>> SelecionarTodos()
    {
        try
        {
            var combustiveis = await repositorioCombustivel.SelecionarRegistrosAsync();
            return Result.Ok(combustiveis);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Ocorreu um erro durante a seleção de todos os registros de combustível."
            );
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}

public class CadastrarCombustivelValidator : AbstractValidator<Combustivel>
{
    public CadastrarCombustivelValidator()
    {
        RuleFor(c => c.Nome)
            .NotEmpty()
            .WithMessage("O campo {PropertyName} não pode ser vazio.");

        RuleFor(c => c.Preco)
            .NotEmpty()
            .WithMessage("O campo {PropertyName} não pode ser vazio.");
    }
}
