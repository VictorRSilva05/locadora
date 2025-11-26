using Locadora.Aplicacao.ModuloCombustivel;
using Locadora.WebApp.Extensions;
using Locadora.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Locadora.WebApp.Controllers;

[Route("combustivel")]
[Authorize(Roles = "Admin, Employee")]
public class CombustivelController : Controller
{
    private readonly CombustivelAppService combustivelAppService;

    public CombustivelController(CombustivelAppService combustivelAppService)
    {
        this.combustivelAppService = combustivelAppService;
    }


    [HttpGet("listar")]
    public async Task<IActionResult> Index()
    {
        var resultado = await combustivelAppService.SelecionarTodos();

        if (resultado.IsFailed)
            return this.RedirecionarParaNotificacaoHome(resultado.ToResult());

        var visualizarVM = new VisualizarCombustivelViewModel(resultado.Value);

        this.ObterNotificacaoPendente();

        return View(visualizarVM);
    }

    [HttpGet("cadastrar")]
    public async Task<IActionResult> Cadastrar()
    {
        var cadastrarVm = new CadastrarCombustivelViewModel();

        return View(cadastrarVm);
    }

    [HttpPost("cadastrar")]
    public async Task<IActionResult> Cadastrar(CadastrarCombustivelViewModel cadastrarVm)
    {
        var entidade = FormularioCombustivelViewModel.ParaEntidade(cadastrarVm);

        var resultado = await combustivelAppService.Cadastrar(entidade);

        if(resultado.IsFailed)
            return this.PreencherErrosModelState(resultado, cadastrarVm);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("editar/{id:guid}")]
    public async Task<IActionResult> Editar(Guid id)
    {
        var resultado =  await combustivelAppService.SelecionarPorId(id);

        if (resultado.IsFailed)
            return this.RedirecionarParaNotificacaoHome(resultado.ToResult());

        var editarVm = new EditarCombustivelViewModel(
            id,
            resultado.Value.Nome,
            resultado.Value.Preco
            );

        return View(editarVm);
    }

    [HttpPost("editar/{id:guid}")]
    public async Task<IActionResult> Editar(Guid id, EditarCombustivelViewModel editarVm)
    {
        var entidadeEditada = FormularioCombustivelViewModel.ParaEntidade(editarVm);

        var resultado = await combustivelAppService.Editar(id, entidadeEditada);

        if (resultado.IsFailed)
            return this.PreencherErrosModelState(resultado, editarVm);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("excluir/{id:guid}")]
    public async Task<IActionResult> Excluir(Guid id)
    {
        var resultado = await combustivelAppService.SelecionarPorId(id);

        if (resultado.IsFailed)
            return this.RedirecionarParaNotificacaoHome(resultado.ToResult());

        var excluirVm = new ExcluirCombustivelViewModel(
            id,
            resultado.Value.Nome
            );

        return View(excluirVm);
    }

    [HttpPost("excluir/{id:guid}")]
    public async Task<IActionResult> Excluir(Guid id, ExcluirCombustivelViewModel excluirVm)
    {
        var resultado = await combustivelAppService.Excluir(excluirVm.Id);

        if (resultado.IsFailed)
            return this.RedirecionarParaNotificacaoHome(resultado);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("detalhes/{id:guid}")]
    public async Task<IActionResult> Detalhes(Guid id)
    {
        var resultado = await combustivelAppService.SelecionarPorId(id);

        if (resultado.IsFailed)
            return this.RedirecionarParaNotificacaoHome(resultado.ToResult());

        var detalhesVm = DetalhesCombustivelViewModel.ParaDetalhesVm(resultado.Value);

        return View(detalhesVm);
    }
}
