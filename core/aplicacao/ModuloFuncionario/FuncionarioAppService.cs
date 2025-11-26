using FluentResults;
using Locadora.Aplicacao.Compartilhado;
using Locadora.Dominio.Autenticacao;
using Locadora.Dominio.Compartilhado;
using Locadora.Dominio.ModuloFuncionario;
using Microsoft.Extensions.Logging;

namespace Locadora.Aplicacao.ModuloFuncionario;
public class FuncionarioAppService
{
    private readonly ITenantProvider tenantProvider;
    private readonly IRepositorioFuncionario repositorioFuncionario;
    private readonly IUnitOfWork unitOfWork;
    private readonly ILogger<FuncionarioAppService> logger;

    public FuncionarioAppService(
        ITenantProvider tenantProvider,
        IRepositorioFuncionario repositorioFuncionario,
        IUnitOfWork unitOfWork,
        ILogger<FuncionarioAppService> logger)
    {
        this.tenantProvider = tenantProvider;
        this.repositorioFuncionario = repositorioFuncionario;
        this.unitOfWork = unitOfWork;
        this.logger = logger;
    }

    public async Task<Result> Cadastrar(Funcionario funcionario)
    {
        try
        {
            funcionario.EmpresaId = tenantProvider.TenantId.GetValueOrDefault();

            await repositorioFuncionario.CadastrarAsync(funcionario);

            funcionario.DataAdmissao = DateTime.SpecifyKind(
                funcionario.DataAdmissao,
                DateTimeKind.Utc
            );

            await unitOfWork.CommitAsync();

            return Result.Ok();
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            logger.LogError(
                ex,
                "Ocorreu um erro durante o registro de {@Registro}.",
                funcionario
            );
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }

    public async Task<Result> Editar(Guid id, Funcionario funcionario)
    {
        try
        {
            await repositorioFuncionario.EditarAsync(id, funcionario);

            funcionario.DataAdmissao = DateTime.SpecifyKind(
                 funcionario.DataAdmissao,
                 DateTimeKind.Utc
            );

            await unitOfWork.CommitAsync();
            return Result.Ok();
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            logger.LogError(
                ex,
                "Ocorreu um erro durante a edição de {@Registro}.",
                funcionario
            );
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }

    public async Task<Result> Excluir(Guid id)
    {
        try
        {
            await repositorioFuncionario.ExcluirAsync(id);
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

    public async Task<Result<Funcionario>> SelecionarPorId(Guid id)
    {
        try
        {
            var funcionario = await repositorioFuncionario.SelecionarRegistroPorIdAsync(id);

            if (funcionario is null)
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(id));

            return Result.Ok(funcionario);
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

    public async Task<Result<List<Funcionario>>> SelecionarTodos()
    {
        try
        {
            var funcionarios = await repositorioFuncionario.SelecionarRegistrosAsync();
            return Result.Ok(funcionarios);
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
