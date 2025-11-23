using FluentResults;
using Locadora.Aplicacao.Compartilhado;
using Locadora.Dominio.Compartilhado;
using Locadora.Dominio.ModuloCliente;
using Microsoft.Extensions.Logging;

namespace Locadora.Aplicacao.ModuloCliente;
public class ClienteAppService
{
    private readonly IRepositorioCliente repositorioCliente;
    private readonly IUnitOfWork unitOfWork;
    private readonly ILogger<ClienteAppService> logger;
    public ClienteAppService(
        IRepositorioCliente repositorioCliente,
        IUnitOfWork unitOfWork,
        ILogger<ClienteAppService> logger)
    {
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

        if(cliente.TipoCliente == TipoClienteEnum.PessoaFisica && cliente.CPF is null)
            return Result.Fail(
                ResultadosErro.RequisicaoInvalidaErro("CPF é obrigatório para clientes do tipo Pessoa Física.")
            );
        if(cliente.TipoCliente == TipoClienteEnum.PessoaJuridica && cliente.CNPJ is null)
            return Result.Fail(
                ResultadosErro.RequisicaoInvalidaErro("CNPJ é obrigatório para clientes do tipo Pessoa Jurídica.")
            );

        try
        {
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
        if (cliente.TipoCliente == TipoClienteEnum.PessoaFisica && cliente.CPF is null)
            return Result.Fail(
                ResultadosErro.RequisicaoInvalidaErro("CPF é obrigatório para clientes do tipo Pessoa Física.")
            );
        if (cliente.TipoCliente == TipoClienteEnum.PessoaJuridica && cliente.CNPJ is null)
            return Result.Fail(
                ResultadosErro.RequisicaoInvalidaErro("CNPJ é obrigatório para clientes do tipo Pessoa Jurídica.")
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
            var cliente =  await repositorioCliente.SelecionarRegistroPorIdAsync(id);
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
            var clientes =  await repositorioCliente.SelecionarRegistrosAsync();
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
