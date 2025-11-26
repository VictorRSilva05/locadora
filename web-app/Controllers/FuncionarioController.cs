using Locadora.Aplicacao.ModuloFuncionario;
using Locadora.Dominio.ModuloFuncionario;
using Locadora.WebApp.Extensions;
using Locadora.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Locadora.WebApp.Controllers;

[Route("funcionarios")]
[Authorize(Roles = "Admin")]
public class FuncionarioController : Controller
{
    private readonly FuncionarioAppService funcionarioAppService;

    public FuncionarioController(FuncionarioAppService funcionarioAppService)
    {
        this.funcionarioAppService = funcionarioAppService;
    }

    [HttpGet("listar")]
    public async Task<IActionResult> Index()
    {
        var resultado = await funcionarioAppService.SelecionarTodos();

        if (resultado.IsFailed)
            return this.RedirecionarParaNotificacaoHome(resultado.ToResult());

        var visualizarVM = new VisualizarFuncionarioViewModel(resultado.Value);

        this.ObterNotificacaoPendente();

        return View(visualizarVM);
    }

    [HttpGet("cadastrar")]
    public async Task<IActionResult> Cadastrar()
    {
        var cadastrarVm = new CadastrarFuncionarioViewModel();

        return View(cadastrarVm);
    }

    [HttpPost("cadastrar")]
    public async Task<IActionResult> Cadastrar(CadastrarFuncionarioViewModel cadastrarVm)
    {
        var entidade = FormularioFuncionarioViewModel.ParaEntidade(cadastrarVm);

        var resultado = await funcionarioAppService.Cadastrar(entidade);

        if(resultado.IsFailed)
            return this.PreencherErrosModelState(resultado, cadastrarVm);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("editar/{id:guid}")]
    public async Task<IActionResult> Editar(Guid id)
    {
        var resultado =  await funcionarioAppService.SelecionarPorId(id);

        if (resultado.IsFailed)
            return this.RedirecionarParaNotificacaoHome(resultado.ToResult());

        var funcionario = resultado.Value;

        var editarVm = new EditarFuncionarioViewModel(
            funcionario.Id,
            funcionario.Nome,
            funcionario.DataAdmissao,
            funcionario.Salario
        );
        return View(editarVm);
    }

    [HttpPost("editar/{id:guid}")]
    public async Task<IActionResult> Editar(Guid id, EditarFuncionarioViewModel editarVm)
    {
        var entidade = FormularioFuncionarioViewModel.ParaEntidade(editarVm);

        var resultado = await funcionarioAppService.Editar(id, entidade);

        if (resultado.IsFailed)
            return this.PreencherErrosModelState(resultado, editarVm);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("excluir/{id:guid}")]
    public async Task<IActionResult> Excluir(Guid id)
    {
        var resultado = await funcionarioAppService.SelecionarPorId(id);

        if (resultado.IsFailed)
            return this.RedirecionarParaNotificacaoHome(resultado.ToResult());

        var funcionario = resultado.Value;

        var excluirVm = new ExcluirFuncionarioViewModel(
            funcionario.Id,
            funcionario.Nome
        );

        return View(excluirVm);
    }

    [HttpPost("excluir/{id:guid}")]
    public async Task<IActionResult> Excluir(Guid id, ExcluirFuncionarioViewModel excluirVm)
    {
        var resultado = await funcionarioAppService.Excluir(id);

        if (resultado.IsFailed)
            return this.PreencherErrosModelState(resultado, excluirVm);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("detalhes/{id:guid}")]
    public async Task<IActionResult> Detalhes(Guid id)
    {
        var resultado = await funcionarioAppService.SelecionarPorId(id);

        if (resultado.IsFailed)
            return this.RedirecionarParaNotificacaoHome(resultado.ToResult());

        var detalhesVm = DetalhesFuncionarioViewModel.ParaDetalhesVm(resultado.Value);

        return View(detalhesVm);
    }
}
