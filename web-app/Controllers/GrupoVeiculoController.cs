using Locadora.Aplicacao.ModuloGrupoVeiculo;
using Locadora.Dominio.ModuloGrupoVeiculo;
using Locadora.WebApp.Extensions;
using Locadora.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static Locadora.WebApp.Models.FormularioGrupoVeiculoViewModel;

namespace Locadora.WebApp.Controllers;

[Route("grupo-veiculo")]
public class GrupoVeiculoController : Controller
{
    private readonly GrupoVeiculoAppService grupoVeiculoAppService;

    public GrupoVeiculoController(GrupoVeiculoAppService grupoVeiculoAppService)
    {
        this.grupoVeiculoAppService = grupoVeiculoAppService;
    }


    [HttpGet("listar")]
    public async Task<IActionResult> Index()
    {
        var resultado = await grupoVeiculoAppService.SelecionarTodos();

        if (resultado.IsFailed)
            return this.RedirecionarParaNotificacaoHome(resultado.ToResult());

        var visualizarVM = new VisualizarGrupoVeiculoViewModel(resultado.Value);

        this.ObterNotificacaoPendente();

        return View(visualizarVM);
    }

    [HttpGet("cadastrar")]
    public async Task<IActionResult> Cadastrar()
    {
        var cadastrarVm = new CadastrarGrupoVeiculoViewModel();

        return View(cadastrarVm);
    }

    [HttpPost("cadastrar")]
    public async Task<IActionResult> Cadastrar(CadastrarGrupoVeiculoViewModel cadastrarVm)
    {
        var entidade = FormularioGrupoVeiculoViewModel.ParaEntidade(cadastrarVm);

        var resultado = await grupoVeiculoAppService.Cadastrar(entidade);

        if(resultado.IsFailed)
            return this.PreencherErrosModelState(resultado, cadastrarVm);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("editar/{id:guid}")]
    public async Task<IActionResult> Editar(Guid id)
    {
        var resultado = await grupoVeiculoAppService.SelecionarPorId(id);

        if (resultado.IsFailed)
            return this.RedirecionarParaNotificacao(resultado.ToResult());

        var editarVm = new EditarGrupoVeiculoViewModel(
            id,
            resultado.Value.Nome
            );

        return View(editarVm);
    }

    [HttpPost("editar/{id:guid}")]
    public async Task<IActionResult> Editar(Guid id, EditarGrupoVeiculoViewModel editarVm)
    {
        var entidadeEditada = FormularioGrupoVeiculoViewModel.ParaEntidade(editarVm);

        var resultado = await grupoVeiculoAppService.Editar(id, entidadeEditada);

        if(resultado.IsFailed)
            return this.PreencherErrosModelState(resultado, editarVm);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("excluir/{id:guid}")]
    public async Task<IActionResult> Excluir(Guid id)
    {
        var resultado = await grupoVeiculoAppService.SelecionarPorId(id);

        if (resultado.IsFailed)
            return this.RedirecionarParaNotificacao(resultado.ToResult());

        var excluirVm = new ExcluirGrupoVeiculoViewModel
        {
            Id = resultado.Value.Id,
            Nome = resultado.Value.Nome
        };

        return View(excluirVm);
    }

    [HttpPost("excluir/{id:guid}")]
    public async Task<IActionResult> Excluir(ExcluirGrupoVeiculoViewModel excluirVm)
    {
        var resultado = await grupoVeiculoAppService.Excluir(excluirVm.Id);

        if (resultado.IsFailed)
            return this.RedirecionarParaNotificacao(resultado);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("detalhes/{id:guid}")]
    public async Task<IActionResult> Detalhes(Guid id)
    {
        var resultado = await grupoVeiculoAppService.SelecionarPorId(id);

        if (resultado.IsFailed)
            return this.RedirecionarParaNotificacao(resultado.ToResult());

        var detalhesVm = DetalhesGrupoVeiculoViewModel.ParaDetalhesVm(resultado.Value);
        
        return View(detalhesVm);
    }
}
