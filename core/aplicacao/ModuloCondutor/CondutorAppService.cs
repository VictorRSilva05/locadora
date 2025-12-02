using FluentResults;
using FluentValidation;
using Locadora.Aplicacao.Compartilhado;
using Locadora.Dominio.Autenticacao;
using Locadora.Dominio.Compartilhado;
using Locadora.Dominio.ModuloAluguel;
using Locadora.Dominio.ModuloCondutor;
using Locadora.Dominio.ModuloTaxa;
using Microsoft.Extensions.Logging;

namespace Locadora.Aplicacao.ModuloCondutor;
public class CondutorAppService
{
    private readonly ITenantProvider tenantProvider;
    private readonly IRepositorioCondutor repositorioCondutor;
    private readonly IRepositorioAluguel repositorioAluguel;
    private readonly IUnitOfWork unitOfWork;
    private readonly IValidator<Condutor> validator;
    private readonly ILogger<CondutorAppService> logger;

    public CondutorAppService(
        ITenantProvider tenantProvider,
        IRepositorioCondutor repositorioCondutor,
        IUnitOfWork unitOfWork,
        ILogger<CondutorAppService> logger,
        IRepositorioAluguel repositorioAluguel,
        IValidator<Condutor> validator)
    {
        this.tenantProvider = tenantProvider;
        this.repositorioCondutor = repositorioCondutor;
        this.unitOfWork = unitOfWork;
        this.logger = logger;
        this.repositorioAluguel = repositorioAluguel;
        this.validator = validator;
    }

    public async Task<Result> Cadastrar(Condutor condutor)
    {
        var resultado = await validator.ValidateAsync(condutor);

        if (!resultado.IsValid)
            return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(resultado.Errors.Select(x => x.ErrorMessage)));

        var registros = await repositorioCondutor.SelecionarRegistrosAsync();

        if (registros.Any(i => i.Cpf == condutor.Cpf))
            return Result.Fail(
                ResultadosErro.RegistroDuplicadoErro("Já existe um condutor cadastrado com este CPF.")
            );

        if (registros.Any(i => i.Cnh == condutor.Cnh))
            return Result.Fail(
                ResultadosErro.RegistroDuplicadoErro("Já existe um condutor cadastrado com esta CNH.")
            );

        if (registros.Any(i => i.Email == condutor.Email))
            return Result.Fail(
                ResultadosErro.RegistroDuplicadoErro("Já existe um condutor cadastrado com este E-mail.")
            );

        try
        {
            condutor.EmpresaId = tenantProvider.TenantId.GetValueOrDefault();

            await repositorioCondutor.CadastrarAsync(condutor);

            condutor.Validade = DateTime.SpecifyKind(condutor.Validade, DateTimeKind.Utc);

            await unitOfWork.CommitAsync();
            return Result.Ok();
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            logger.LogError(
                ex,
                "Ocorreu um erro durante o registro de {@Registro}.",
                condutor
            );
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }

    public async Task<Result> Editar(Guid id, Condutor condutor)
    {
        var resultado = await validator.ValidateAsync(condutor);

        if (!resultado.IsValid)
            return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(resultado.Errors.Select(x => x.ErrorMessage)));

        var registros = await repositorioCondutor.SelecionarRegistrosAsync();
        if (registros.Any(i => i.Cpf == condutor.Cpf && i.Id != id))
            return Result.Fail(
                ResultadosErro.RegistroDuplicadoErro("Já existe um condutor cadastrado com este CPF.")
            );
        if (registros.Any(i => i.Cnh == condutor.Cnh && i.Id != id))
            return Result.Fail(
                ResultadosErro.RegistroDuplicadoErro("Já existe um condutor cadastrado com esta CNH.")
            );
        if (registros.Any(i => i.Email == condutor.Email && i.Id != id))
            return Result.Fail(
                ResultadosErro.RegistroDuplicadoErro("Já existe um condutor cadastrado com este E-mail.")
            );
        try
        {
            await repositorioCondutor.EditarAsync(id, condutor);

            condutor.Validade = DateTime.SpecifyKind(condutor.Validade, DateTimeKind.Utc);

            await unitOfWork.CommitAsync();
            return Result.Ok();
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            logger.LogError(
                ex,
                "Ocorreu um erro durante a edição de {@Registro}.",
                condutor
            );
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }

    public async Task<Result> Excluir(Guid id)
    {
        var alugueis = (await repositorioAluguel.SelecionarRegistrosAsync()) ?? new List<Aluguel>();

        if (alugueis.Any(a => a.Status && a.Condutor!.Id == id))
            return Result.Fail(ResultadosErro.ExclusaoBloqueadaErro("Este condutor está presente em um aluguel."));

        try
        {
            await repositorioCondutor.ExcluirAsync(id);
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

    public async Task<Result<Condutor>> SelecionarPorId(Guid id)
    {
        try
        {
            var condutor = await repositorioCondutor.SelecionarRegistroPorIdAsync(id);

            if (repositorioCondutor == null)
                return Result.Fail(
                    ResultadosErro.RegistroNaoEncontradoErro(id)
                );

            return Result.Ok(condutor);
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

    public async Task<Result<List<Condutor>>> SelecionarTodos()
    {
        try
        {
            var condutores = await repositorioCondutor.SelecionarRegistrosAsync();
            return Result.Ok(condutores);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Ocorreu um erro durante a seleção dos registros de condutor."
            );
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}

public class CadastrarCondutorValidator : AbstractValidator<Condutor>
{
    public CadastrarCondutorValidator()
    {
        RuleFor(c => c.Nome)
           .NotEmpty()
           .WithMessage("O campo {PropertyName} não pode ser vazio.");

        RuleFor(c => c.Email)
             .EmailAddress()
             .WithMessage("O formato do {PropertyName} está incorreto");

        RuleFor(c => c.Cpf)
            .Matches(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$")
            .WithMessage("O formato do {PropertyName} está incorreto");

        RuleFor(c => c.Cnh)
            .Matches(@"^\d{11}$")
            .WithMessage("O formato do {PropertyName} está incorreto");
    }
}
