using Locadora.Aplicacao.ModuloCobranca;
using Locadora.Aplicacao.ModuloGrupoVeiculo;
using Locadora.WebApp.Extensions;
using Locadora.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace Locadora.WebApp.Controllers;

[Route("cobrancas")]
public class CobrancaController : Controller
{
    private readonly CobrancaAppService cobrancaAppService;
    private readonly GrupoVeiculoAppService grupoVeiculoAppService;

    public CobrancaController(
        CobrancaAppService cobrancaAppService,
        GrupoVeiculoAppService grupoVeiculoAppService)
    {
        this.cobrancaAppService = cobrancaAppService;
        this.grupoVeiculoAppService = grupoVeiculoAppService;
    }

    [HttpGet("listar")]
    public async Task<IActionResult> Index()
    {
        var resultado = await cobrancaAppService.SelecionarTodos();

        if (resultado.IsFailed)
            return this.RedirecionarParaNotificacaoHome(resultado.ToResult());

        var visualizarVM = new VisualizarCobrancaViewModel(resultado.Value);

        this.ObterNotificacaoPendente();

        return View(visualizarVM);
    }

    [HttpGet("cadastrar")]
    public async Task<IActionResult> Cadastrar()
    {
        var grupoVeiculosResultado = await grupoVeiculoAppService.SelecionarTodos();

        var cadastrarVm = new CadastrarCobrancaViewModel(
            grupoVeiculosResultado.ValueOrDefault);

        return View(cadastrarVm);
    }

    [HttpPost("cadastrar")]
    public async Task<IActionResult> Cadastrar(CadastrarCobrancaViewModel cadastrarVm)
    {
        var grupoVeiculosResultado = await grupoVeiculoAppService.SelecionarTodos();
        
        var entidade = FormularioCobrancaViewModel.ParaEntidade(cadastrarVm, grupoVeiculosResultado.ValueOrDefault);

        var resultado = await cobrancaAppService.Cadastrar(entidade);

        if(resultado.IsFailed)
            return this.PreencherErrosModelState(resultado, cadastrarVm);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("editar/{id:guid}")]
    public async Task<IActionResult> Editar(Guid id)
    {
        var grupoVeiculosResultado = await grupoVeiculoAppService.SelecionarTodos();

        var cobrancaResultado = await cobrancaAppService.SelecionarPorId(id);

        if (cobrancaResultado.IsFailed)
            return this.RedirecionarParaNotificacaoHome(cobrancaResultado.ToResult());

        var editarVm = new EditarCobrancaViewModel(
            cobrancaResultado.Value!,
            grupoVeiculosResultado.ValueOrDefault);

        return View(editarVm);
    }

    [HttpPost("editar/{id:guid}")]
    public async Task<IActionResult> Editar(Guid id, EditarCobrancaViewModel editarVm)
    {
        var grupoVeiculosResultado = await grupoVeiculoAppService.SelecionarTodos();

        var entidade = FormularioCobrancaViewModel.ParaEntidade(editarVm, grupoVeiculosResultado.ValueOrDefault);

        entidade.Id = editarVm.Id;

        var resultado = await cobrancaAppService.Editar(id, entidade);

        if (resultado.IsFailed)
            return this.PreencherErrosModelState(resultado, editarVm);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("excluir/{id:guid}")]
    public async Task<IActionResult> Excluir(Guid id)
    {
        var cobrancaResultado = await cobrancaAppService.SelecionarPorId(id);

        if (cobrancaResultado.IsFailed)
            return this.RedirecionarParaNotificacaoHome(cobrancaResultado.ToResult());

        var excluirVm = new ExcluirCobrancaViewModel(cobrancaResultado.Value!);

        return View(excluirVm);
    }

    [HttpPost("excluir/{id:guid}")]
    public async Task<IActionResult> Excluir(Guid id, ExcluirCobrancaViewModel excluirVm)
    {
        var resultado = await cobrancaAppService.Excluir(id);

        if (resultado.IsFailed)
            return this.PreencherErrosModelState(resultado, excluirVm);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("detalhes/{id:guid}")]
    public async Task<IActionResult> Detalhes(Guid id)
    {
        var cobrancaResultado = await cobrancaAppService.SelecionarPorId(id);

        if (cobrancaResultado.IsFailed)
            return this.RedirecionarParaNotificacaoHome(cobrancaResultado.ToResult());

        var detalhesVm = DetalhesCobrancaViewModel.ParaDetalhesVm(cobrancaResultado.Value!);

        return View(detalhesVm);
    }
}
