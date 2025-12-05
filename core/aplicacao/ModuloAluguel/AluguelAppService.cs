using FluentResults;
using Locadora.Aplicacao.Compartilhado;
using Locadora.Dominio.Autenticacao;
using Locadora.Dominio.Compartilhado;
using Locadora.Dominio.ModuloAluguel;
using Locadora.Dominio.ModuloCobranca;
using Microsoft.Extensions.Logging;

namespace Locadora.Aplicacao.ModuloAluguel;
public class AluguelAppService
{
    private readonly ITenantProvider tenantProvider;
    private readonly IRepositorioAluguel repositorioAluguel;
    private readonly IRepositorioDevolucao repositorioDevolucao;
    private readonly IUnitOfWork unitOfWork;
    private readonly ILogger<AluguelAppService> logger;

    public AluguelAppService(
        ITenantProvider tenantProvider,
        IRepositorioAluguel repositorioAluguel,
        IUnitOfWork unitOfWork,
        ILogger<AluguelAppService> logger,
        IRepositorioDevolucao repositorioDevolucao)
    {
        this.tenantProvider = tenantProvider;
        this.repositorioAluguel = repositorioAluguel;
        this.unitOfWork = unitOfWork;
        this.logger = logger;
        this.repositorioDevolucao = repositorioDevolucao;
    }

    public async Task<Result> Cadastrar(Aluguel aluguel)
    {
        try
        {
            aluguel.EmpresaId = tenantProvider.TenantId.GetValueOrDefault();

            aluguel.Veiculo.Ocupar();

            await repositorioAluguel.CadastrarAsync(aluguel);

            aluguel.DataSaida = DateTime.SpecifyKind(
                aluguel.DataSaida,
                DateTimeKind.Utc
                );

            aluguel.DataRetornoPrevista = DateTime.SpecifyKind(
                aluguel.DataRetornoPrevista,
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
                aluguel
                );
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }

    public async Task<Result> RegistrarDevolucao(Devolucao devolucao, Aluguel aluguel)
    {
        try
        {
            devolucao.EmpresaId = tenantProvider.TenantId.GetValueOrDefault();

            aluguel.Veiculo.Desocupar();
            aluguel.Status = false;
            aluguel.Veiculo.Kilometragem = devolucao.KmDevolucao;

            devolucao.DataDevolucao = DateTime.SpecifyKind(
                 devolucao.DataDevolucao,
                 DateTimeKind.Utc
                 );

            devolucao.Total= aluguel.CalculcarTotal(devolucao);

            await repositorioDevolucao.CadastrarAsync(devolucao);

            await unitOfWork.CommitAsync();
            return Result.Ok();
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();

            logger.LogError(
                ex,
                "Erro ao registrar devolução para o aluguel {@Id}.",
                devolucao.Id
            );

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }



    public async Task<Result> Editar(Guid id, Aluguel aluguel)
    {
        try
        {
            await repositorioAluguel.EditarAsync(id, aluguel);


            aluguel.DataSaida = DateTime.SpecifyKind(
                aluguel.DataSaida,
                DateTimeKind.Utc
                );

            aluguel.DataRetornoPrevista = DateTime.SpecifyKind(
                aluguel.DataRetornoPrevista,
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
           aluguel
       );
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }

    public async Task<Result> Excluir(Guid id)
    {
        try
        {
            await repositorioAluguel.ExcluirAsync(id);
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

    public async Task<Result<Aluguel>> SelecionarPorId(Guid id)
    {
        try
        {
            var aluguel = await repositorioAluguel.SelecionarRegistroPorIdAsync(id);

            if (aluguel is null)
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(id));

            return Result.Ok(aluguel);
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

    public async Task<Result<List<Aluguel>>> SelecionarTodos()
    {
        try
        {
            var alugueis = await repositorioAluguel.SelecionarRegistrosAsync();
            return Result.Ok(alugueis);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Ocorreu um erro durante a seleção dos registros de aluguel."
            );
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}