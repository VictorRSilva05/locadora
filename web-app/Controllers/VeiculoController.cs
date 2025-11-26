using Locadora.Aplicacao.ModuloCombustivel;
using Locadora.Aplicacao.ModuloGrupoVeiculo;
using Locadora.Aplicacao.ModuloVeiculo;
using Locadora.WebApp.Extensions;
using Locadora.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Locadora.WebApp.Controllers;

[Route("veiculos")]
[Authorize(Roles = "Admin, Employee")]
public class VeiculoController : Controller
{
    private readonly VeiculoAppService veiculoAppService;
    private readonly GrupoVeiculoAppService grupoVeiculoAppService;
    private readonly CombustivelAppService combustivelAppService;

    public VeiculoController(
        VeiculoAppService veiculoAppService,
        GrupoVeiculoAppService grupoVeiculoAppService,
        CombustivelAppService combustivelAppService)
    {
        this.veiculoAppService = veiculoAppService;
        this.grupoVeiculoAppService = grupoVeiculoAppService;
        this.combustivelAppService = combustivelAppService;
    }

    [HttpGet("listar")]
    public async Task<IActionResult> Index()
    {
        var resultado = await veiculoAppService.SelecionarTodos();

        if (resultado.IsFailed)
            return this.RedirecionarParaNotificacaoHome(resultado.ToResult());

        var visualizarVM = new VisualizarVeiculoViewModel(resultado.Value);

        this.ObterNotificacaoPendente();

        return View(visualizarVM);
    }

    [HttpGet("cadastrar")]
    public async Task<IActionResult> Cadastrar()
    {
        var grupoVeiculosResultado = await grupoVeiculoAppService.SelecionarTodos();

        var combustiveisResultado = await combustivelAppService.SelecionarTodos();

        var cadastrarVm = new CadastrarVeiculoViewModel(
            grupoVeiculosResultado.ValueOrDefault,
            combustiveisResultado.ValueOrDefault);

        return View(cadastrarVm);
    }

    [HttpPost("cadastrar")]
    public async Task<IActionResult> Cadastrar(CadastrarVeiculoViewModel cadastrarVm)
    {
        var grupoVeiculosResultado = await grupoVeiculoAppService.SelecionarTodos();

        var combustiveisResultado = await combustivelAppService.SelecionarTodos();

        var entidade = FormularioVeiculoViewModel.ParaEntidade(cadastrarVm, grupoVeiculosResultado.ValueOrDefault, combustiveisResultado.ValueOrDefault);

        var resultado = await veiculoAppService.Cadastrar(entidade);

        if (resultado.IsFailed)
            return this.PreencherErrosModelState(resultado, cadastrarVm);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("editar/{id:guid}")]
    public async Task<IActionResult> Editar(Guid id)
    {
        var resultado = await veiculoAppService.SelecionarPorId(id);

        if (resultado.IsFailed)
            return this.RedirecionarParaNotificacaoHome(resultado.ToResult());

        var grupoVeiculosResultado = await grupoVeiculoAppService.SelecionarTodos();
        var combustiveisResultado = await combustivelAppService.SelecionarTodos();

        var veiculo = resultado.Value;
        var editarVm = new EditarVeiculoViewModel(
            veiculo,
            grupoVeiculosResultado.ValueOrDefault,
            combustiveisResultado.ValueOrDefault
        );

        return View(editarVm);
    }


    [HttpPost("editar/{id:guid}")]
    public async Task<IActionResult> Editar(Guid id, EditarVeiculoViewModel editarVm)
    {
        var grupoVeiculosResultado = await grupoVeiculoAppService.SelecionarTodos();

        var combustiveisResultado = await combustivelAppService.SelecionarTodos();

        var entidade = FormularioVeiculoViewModel.ParaEntidade(editarVm, grupoVeiculosResultado.ValueOrDefault, combustiveisResultado.ValueOrDefault);

        entidade.Id = editarVm.Id;

        var resultado = await veiculoAppService.Editar(id,entidade);

        if (resultado.IsFailed)
            return this.PreencherErrosModelState(resultado, editarVm);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("excluir/{id:guid}")]
    public async Task<IActionResult> Excluir(Guid id)
    {
        var resultado =  await veiculoAppService.SelecionarPorId(id);  
        
        if (resultado.IsFailed)
            return this.RedirecionarParaNotificacaoHome(resultado.ToResult());

        var veiculo = resultado.Value;
        var excluirVm = new ExcluirVeiculoViewModel(
            veiculo.Id,
            veiculo.Modelo
        );
        return View(excluirVm);
    }

    [HttpPost("excluir/{id:guid}")]
    public async Task<IActionResult> Excluir(Guid id, ExcluirVeiculoViewModel excluirVm)
    {
        var resultado = await veiculoAppService.Excluir(id);

        if (resultado.IsFailed)
            return this.PreencherErrosModelState(resultado, excluirVm);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("detalhes/{id:guid}")]
    public async Task<IActionResult> Detalhes(Guid id)
    {
        var resultado =  await veiculoAppService.SelecionarPorId(id);   

        if (resultado.IsFailed)
            return this.RedirecionarParaNotificacaoHome(resultado.ToResult());

        var detalhesVm = DetalhesVeiculoViewModel.ParaDetalhesVm(resultado.Value);

        return View(detalhesVm);
    }
}
