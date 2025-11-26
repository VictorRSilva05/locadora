using Locadora.Aplicacao.ModuloCondutor;
using Locadora.WebApp.Extensions;
using Locadora.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Locadora.WebApp.Controllers;

[Route("condutores")]
[Authorize(Roles = "Admin, Employee")]
public class CondutorController : Controller
{
    private readonly CondutorAppService condutorAppService;
    public CondutorController(CondutorAppService condutorAppService)
    {
        this.condutorAppService = condutorAppService;
    }

    [HttpGet("listar")]
    public async Task<IActionResult> Index()
    {
        var resultado = await condutorAppService.SelecionarTodos();

        if (resultado.IsFailed)
            return this.RedirecionarParaNotificacaoHome(resultado.ToResult());

        var visualizarVM = new VisualizarCondutorViewModel(resultado.Value);

        this.ObterNotificacaoPendente();

        return View(visualizarVM);
    }

    [HttpGet("cadastrar")]
    public async Task<IActionResult> Cadastrar()
    {
        var cadastrarVm = new CadastrarCondutorViewModel();

        return View(cadastrarVm);
    }

    [HttpPost("cadastrar")]
    public async Task<IActionResult> Cadastrar(CadastrarCondutorViewModel cadastrarVm)
    {
        var entidade = FormularioCondutorViewModel.ParaEntidade(cadastrarVm);

        var resultado = await condutorAppService.Cadastrar(entidade);

        if (resultado.IsFailed)
            return this.PreencherErrosModelState(resultado, cadastrarVm);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("editar/{id:guid}")]
    public async Task<IActionResult> Editar(Guid id)
    {
        var resultado = await condutorAppService.SelecionarPorId(id);
        if (resultado.IsFailed)
            return this.RedirecionarParaNotificacaoHome(resultado.ToResult());

        var condutor = resultado.Value;

        var editarVm = new EditarCondutorViewModel(
            condutor.Id,
            condutor.Nome,
            condutor.Email,
            condutor.Telefone,
            condutor.Cpf,
            condutor.Cnh,
            condutor.Validade
        );

        return View(editarVm);
    }

    [HttpPost("editar/{id:guid}")]
    public async Task<IActionResult> Editar(Guid id, EditarCondutorViewModel editarVm)
    {
        var entidade = FormularioCondutorViewModel.ParaEntidade(editarVm);

        var resultado = await condutorAppService.Editar(id, entidade);

        if (resultado.IsFailed)
            return this.PreencherErrosModelState(resultado, editarVm);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("excluir/{id:guid}")]
    public async Task<IActionResult> Excluir(Guid id)
    {
        var resultado = await condutorAppService.SelecionarPorId(id);
        
        if (resultado.IsFailed)
            return this.RedirecionarParaNotificacaoHome(resultado.ToResult());

        var condutor = resultado.Value;

        var excluirVm = new ExcluirCondutorViewModel(
            condutor.Id,
            condutor.Nome
        );
        return View(excluirVm);
    }

    [HttpPost("excluir/{id:guid}")]
    public async Task<IActionResult> Excluir(Guid id, ExcluirCondutorViewModel excluirVm)
    {
        var resultado = await condutorAppService.Excluir(id);

        if (resultado.IsFailed)
            return this.PreencherErrosModelState(resultado, excluirVm);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("detalhes/{id:guid}")]
    public async Task<IActionResult> Detalhes(Guid id)
    {
        var resultado = await condutorAppService.SelecionarPorId(id);

        if (resultado.IsFailed)
            return this.RedirecionarParaNotificacaoHome(resultado.ToResult());

        var condutor = resultado.Value;

        var detalhesVm = DetalhesCondutorViewModel.ParaDetalhesVm(resultado.Value);
      
        return View(detalhesVm);
    }
}
