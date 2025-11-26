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
    private readonly IUnitOfWork unitOfWork;
    private readonly ILogger<AluguelAppService> logger;

    public AluguelAppService(
        ITenantProvider tenantProvider,
        IRepositorioAluguel repositorioAluguel,
        IUnitOfWork unitOfWork,
        ILogger<AluguelAppService> logger)
    {
        this.tenantProvider = tenantProvider;
        this.repositorioAluguel = repositorioAluguel;
        this.unitOfWork = unitOfWork;
        this.logger = logger;
    }

    public async Task<Result> Cadastrar(Aluguel aluguel)
    {
        try
        {
            aluguel.EmpresaId = tenantProvider.TenantId.GetValueOrDefault();

            await repositorioAluguel.CadastrarAsync(aluguel);

            aluguel.DataSaida = DateTime.SpecifyKind(
                aluguel.DataSaida,
                DateTimeKind.Utc
                );

            aluguel.DataRetornoPrevista = DateTime.SpecifyKind(
                aluguel.DataRetornoPrevista,
                DateTimeKind.Utc
                );

            aluguel.DataDevolucao = aluguel.DataDevolucao.HasValue
                ? DateTime.SpecifyKind(aluguel.DataDevolucao.Value, DateTimeKind.Utc)
                : null;

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

    public async Task<Result> RegistrarDevolucao(
           Guid id,
           DateTime dataDevolucao,
           float kmDevolucao,
           bool tanqueCheio,
           bool seguroAcionado,
           decimal total)
    {
        try
        {
            var aluguel = await repositorioAluguel.SelecionarRegistroPorIdAsync(id);

            if (aluguel is null)
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(id));

            // Aplica as alterações de devolução na entidade
            aluguel.DataDevolucao = DateTime.SpecifyKind(dataDevolucao, DateTimeKind.Utc);
            aluguel.KmDevolucao = kmDevolucao;
            aluguel.TanqueCheio = tanqueCheio;
            aluguel.SeguroAcionado = seguroAcionado;
            aluguel.Total = total;

            // Atualiza o registro
            await repositorioAluguel.EditarAsync(id, aluguel);

            await unitOfWork.CommitAsync();
            return Result.Ok();
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();

            logger.LogError(
                ex,
                "Erro ao registrar devolução para o aluguel {@Id}.",
                id
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

            aluguel.DataDevolucao = aluguel.DataDevolucao is DateTime dt
                 ? DateTime.SpecifyKind(dt, DateTimeKind.Utc)
                 : null;

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