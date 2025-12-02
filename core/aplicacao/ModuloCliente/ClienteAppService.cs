using FluentResults;
using FluentValidation;
using Locadora.Aplicacao.Compartilhado;
using Locadora.Dominio.Autenticacao;
using Locadora.Dominio.Compartilhado;
using Locadora.Dominio.ModuloCliente;
using Microsoft.Extensions.Logging;

namespace Locadora.Aplicacao.ModuloCliente;
public class ClienteAppService
{
    ITenantProvider tenantProvider;
    private readonly IRepositorioCliente repositorioCliente;
    private readonly IUnitOfWork unitOfWork;
    private readonly ILogger<ClienteAppService> logger;
    public ClienteAppService(
        ITenantProvider tenantProvider,
        IRepositorioCliente repositorioCliente,
        IUnitOfWork unitOfWork,
        ILogger<ClienteAppService> logger)
    {
        this.tenantProvider = tenantProvider;
        this.repositorioCliente = repositorioCliente;
        this.unitOfWork = unitOfWork;
        this.logger = logger;
    }

    public async Task<Result> Cadastrar(Cliente cliente)
    {
        var registros = await repositorioCliente.SelecionarRegistrosAsync();

        if (registros.Any(i => i.CPF == cliente.CPF))
            return Result.Fail(
                ResultadosErro.RegistroDuplicadoErro("Já existe um cliente cadastrado com este CPF.")
            );
        if (registros.Any(i => i.Email == cliente.Email))
            return Result.Fail(
                ResultadosErro.RegistroDuplicadoErro("Já existe um cliente cadastrado com este E-mail.")
            );
        if (registros.Any(i => i.CNH == cliente.CNH))
            return Result.Fail(
                ResultadosErro.RegistroDuplicadoErro("Já existe um cliente cadastrado com esta CNH.")
            );
        if (registros.Any(i => i.RG == cliente.RG))
            return Result.Fail(
                ResultadosErro.RegistroDuplicadoErro("Já existe um cliente cadastrado com este RG.")
            );
        if (registros.Any(i => i.CNPJ == cliente.CNPJ))
            return Result.Fail(
                ResultadosErro.RegistroDuplicadoErro("Já existe um cliente cadastrado com este CNPJ.")
            );

        try
        {
            cliente.EmpresaId = tenantProvider.TenantId.GetValueOrDefault();

            await repositorioCliente.CadastrarAsync(cliente);
            await unitOfWork.CommitAsync();
            return Result.Ok();
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            logger.LogError(
                ex,
                "Ocorreu um erro durante o registro de {@Registro}.",
                cliente
            );
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }

    public async Task<Result> Editar(Guid id, Cliente cliente)
    {
        var registros = await repositorioCliente.SelecionarRegistrosAsync();
        if (registros.Any(i => i.CPF == cliente.CPF && i.Id != id))
            return Result.Fail(
                ResultadosErro.RegistroDuplicadoErro("Já existe um cliente cadastrado com este CPF.")
            );
        if (registros.Any(i => i.Email == cliente.Email && i.Id != id))
            return Result.Fail(
                ResultadosErro.RegistroDuplicadoErro("Já existe um cliente cadastrado com este E-mail.")
            );
        if (registros.Any(i => i.Id != id && i.CNH == cliente.CNH))
            return Result.Fail(
                ResultadosErro.RegistroDuplicadoErro("Já existe um cliente cadastrado com esta CNH.")
            );
        if (registros.Any(i => i.Id != id && i.RG == cliente.RG))
            return Result.Fail(
                ResultadosErro.RegistroDuplicadoErro("Já existe um cliente cadastrado com este RG.")
            );
        if (registros.Any(i => i.Id != id && i.CNPJ == cliente.CNPJ))
            return Result.Fail(
                ResultadosErro.RegistroDuplicadoErro("Já existe um cliente cadastrado com este CNPJ.")
                );
        try
        {
            await repositorioCliente.EditarAsync(id, cliente);
            await unitOfWork.CommitAsync();
            return Result.Ok();
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            logger.LogError(
                ex,
                "Ocorreu um erro durante a edição de {@Registro}.",
                cliente
            );
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }

    public async Task<Result> Excluir(Guid id)
    {
        try
        {
            await repositorioCliente.ExcluirAsync(id);
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

    public async Task<Result<Cliente>> SelecionarPorId(Guid id)
    {
        try
        {
            var cliente = await repositorioCliente.SelecionarRegistroPorIdAsync(id);
            return Result.Ok(cliente);
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

    public async Task<Result<List<Cliente>>> SelecionarTodos()
    {
        try
        {
            var clientes = await repositorioCliente.SelecionarRegistrosAsync();
            return Result.Ok(clientes);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Ocorreu um erro durante a seleção dos registros de Cliente."
            );
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}

public class CadastrarClienteValidator : AbstractValidator<Cliente>
{
    public CadastrarClienteValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage("O campo {PropertyName} é obrigatório.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("O campo e-mail é obrigatório.");

        RuleFor(x => x.Telefone)
            .NotEmpty()
            .WithMessage("O campo {PropertyName} é obrigatório.");

        RuleFor(x => x.TipoCliente)
            .IsInEnum()
            .NotEmpty()
            .WithMessage("O campo tipo cliente é obrigatório.");

        RuleFor(x => x.Estado)
            .NotEmpty()
            .WithMessage("O campo {PropertyName} é obrigatório.");

        RuleFor(x => x.Cidade)
            .NotEmpty()
            .WithMessage("O campo {PropertyName} é obrigatório.");

        RuleFor(x => x.Bairro)
            .NotEmpty()
            .WithMessage("O campo {PropertyName} é obrigatório.");

        RuleFor(x => x.Rua)
            .NotEmpty()
            .WithMessage("O campo {PropertyName} é obrigatório.");

        RuleFor(x => x.Numero)
            .NotEmpty()
            .WithMessage("O campo {PropertyName} é obrigatório.");

        When(x => x.TipoCliente == TipoClienteEnum.PessoaJuridica, () =>
        {
            RuleFor(x => x.CNPJ)
                .NotEmpty()
                .Matches("^\\d{2}\\.?\\d{3}\\.?\\d{3}/?\\d{4}-?\\d{2}$\r\n");
        });

        When(x => x.TipoCliente == TipoClienteEnum.PessoaFisica, () =>
        {
            RuleFor(c => c.CPF)
                 .NotEmpty()
                 .WithMessage("O campo {PropertyName} não pode ser vazio.")
                 .DependentRules(() =>
                 {
                     RuleFor(c => c.CPF)
                     .Matches("^\\d{3}\\.?\\d{3}\\.?\\d{3}-?\\d{2}$\r\n");
                 });

            RuleFor(c => c.CNH)
                .NotEmpty()
                .WithMessage("O campo {PropertyName} não pode ser vazio.")
                .DependentRules(() =>
                {
                    RuleFor(c => c.CNH)
                    .Matches("^\\d{11}$\r\n");
                });

            RuleFor(x => x.RG)
                .NotEmpty()
                .WithMessage("O campo {PropertyName} não pode ser vazio.");
        });
    }
}
