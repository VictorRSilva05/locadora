using Locadora.Aplicacao.ModuloTaxa;
using Locadora.WebApp.Extensions;
using Locadora.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Locadora.WebApp.Controllers;

[Route("taxas")]
[Authorize(Roles = "Admin, Employee")]
public class TaxaController : Controller
{
    private readonly TaxaAppService taxaAppService;

    public TaxaController(TaxaAppService taxaAppService)
    {
        this.taxaAppService = taxaAppService;
    }

    [HttpGet("listar")]
    public async Task<IActionResult> Index()
    {
        var resultado = await taxaAppService.SelecionarTodos();

        if (resultado.IsFailed)
            return this.RedirecionarParaNotificacaoHome(resultado.ToResult());

        var visualizarVM = new VisualizarTaxaViewModel(resultado.Value);

        this.ObterNotificacaoPendente();

        return View(visualizarVM);
    }

    [HttpGet("cadastrar")]
    public async Task<IActionResult> Cadastrar()
    {
        var cadastrarVm = new CadastrarTaxaViewModel();
        return View(cadastrarVm);
    }

    [HttpPost("cadastrar")]
    public async Task<IActionResult> Cadastrar(CadastrarTaxaViewModel cadastrarVm)
    {
        var entidade = FormularioTaxaViewModel.ParaEntidade(cadastrarVm);

        var resultado = await taxaAppService.Cadastrar(entidade);

        if (resultado.IsFailed)
            return this.PreencherErrosModelState(resultado, cadastrarVm);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("editar/{id:guid}")]
    public async Task<IActionResult> Editar(Guid id)
    {
        var resultado = await taxaAppService.SelecionarPorId(id);

        if (resultado.IsFailed)
            return this.RedirecionarParaNotificacaoHome(resultado.ToResult());
    
        var editarVm = new EditarTaxaViewModel(
            id,
            resultado.Value.Descricao,
            resultado.Value.Valor,
            resultado.Value.PlanoCobranca);

        editarVm.CarregarPlanosCobranca();

        return View(editarVm);
    }

    [HttpPost("editar/{id:guid}")]
    public async Task<IActionResult> Editar(Guid id, EditarTaxaViewModel editarVm)
    {
        var entidade = FormularioTaxaViewModel.ParaEntidade(editarVm);

        var resultado = await taxaAppService.Editar(id, entidade);

        if (resultado.IsFailed)
            return this.PreencherErrosModelState(resultado, editarVm);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("excluir/{id:guid}")]
    public async Task<IActionResult> Excluir(Guid id)
    {
        var resultado = await taxaAppService.SelecionarPorId(id);

        if (resultado.IsFailed)
            return this.RedirecionarParaNotificacaoHome(resultado.ToResult());

        var taxa = resultado.Value;

        var excluirVm = new ExcluirTaxaViewModel(
            id,
            taxa.Descricao);

        return View(excluirVm);
    }

    [HttpPost("excluir/{id:guid}")]
    public async Task<IActionResult> Excluir(Guid id, ExcluirTaxaViewModel excluirVm)
    {
        var resultado = await taxaAppService.Excluir(excluirVm.Id);

        if (resultado.IsFailed)
            return this.PreencherErrosModelState(resultado, excluirVm);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("detalhes/{id:guid}")]
    public async Task<IActionResult> Detalhes(Guid id)
    {
        var resultado = await taxaAppService.SelecionarPorId(id);
        
        if(resultado.IsFailed)
            return this.RedirecionarParaNotificacaoHome(resultado.ToResult());

        var detalhesVm = DetalhesTaxaViewModel.ParaDetalhesVm(resultado.Value);

        return View(detalhesVm);
    }
}
