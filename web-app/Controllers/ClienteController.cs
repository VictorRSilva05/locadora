using Locadora.Aplicacao.ModuloCliente;
using Locadora.Dominio.ModuloCliente;
using Locadora.WebApp.Extensions;
using Locadora.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Locadora.WebApp.Controllers;

[Route("clientes")]
[Authorize(Roles = "Admin, Employee")]
public class ClienteController : Controller
{
    private readonly ClienteAppService clienteAppService;
    public ClienteController(ClienteAppService clienteAppService)
    {
        this.clienteAppService = clienteAppService;
    }

    [HttpGet("listar")]
    public async Task<IActionResult> Index()
    {
        var resultado = await clienteAppService.SelecionarTodos();

        if (resultado.IsFailed)
            return this.RedirecionarParaNotificacaoHome(resultado.ToResult());

        var visualizarVM = new VisualizarClienteViewModel(resultado.Value);

        this.ObterNotificacaoPendente();

        return View(visualizarVM);
    }

    [HttpGet("cadastrar")]
    public async Task<IActionResult> Cadastrar()
    {
        var cadastrarVm = new CadastrarClienteViewModel();
        cadastrarVm.CarregarTiposCliente();

        return View(cadastrarVm);
    }

    [HttpPost("cadastrar")]
    public async Task<IActionResult> Cadastrar(CadastrarClienteViewModel cadastrarVm)
    {
        var entidade = FormularioClienteViewModel.ParaEntidade(cadastrarVm);

        var resultado = await clienteAppService.Cadastrar(entidade);

        if (resultado.IsFailed)
            return this.PreencherErrosModelState(resultado, cadastrarVm);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("editar/{id:guid}")]
    public async Task<IActionResult> Editar(Guid id)
    {
        var resultado = await clienteAppService.SelecionarPorId(id);

        if (resultado.IsFailed)
            return this.RedirecionarParaNotificacaoHome(resultado.ToResult());

        var cliente = resultado.Value;

        var editarVm = new EditarClienteViewModel(
            cliente.Id,
            cliente.Nome,
            cliente.Email,
            cliente.Telefone,
            cliente.TipoCliente,
            cliente.CPF,
            cliente.CNPJ,
            cliente.Estado,
            cliente.Cidade,
            cliente.Bairro,
            cliente.Rua,
            cliente.Numero
            );

        editarVm.CarregarTiposCliente();

        return View(editarVm);
    }

    [HttpPost("editar/{id:guid}")]
    public async Task<IActionResult> Editar(Guid id, EditarClienteViewModel editarVm)
    {
        var entidade = FormularioClienteViewModel.ParaEntidade(editarVm);

        var resultado = await clienteAppService.Editar(id, entidade);

        if (resultado.IsFailed)
            return this.PreencherErrosModelState(resultado, editarVm);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("excluir/{id:guid}")]
    public async Task<IActionResult> Excluir(Guid id)
    {
        var resultado = await clienteAppService.SelecionarPorId(id);

        if (resultado.IsFailed)
            return this.RedirecionarParaNotificacaoHome(resultado.ToResult());

        var cliente = resultado.Value;

        var excluirVm = new ExcluirClienteViewModel(
            id,
            cliente.Nome
            );
        return View(excluirVm);
    }

    [HttpPost("excluir/{id:guid}")]
    public async Task<IActionResult> Excluir(Guid id, ExcluirClienteViewModel excluirVm)
    {
        var resultado = await clienteAppService.Excluir(id);

        if (resultado.IsFailed)
            return this.PreencherErrosModelState(resultado, excluirVm);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("detalhes/{id:guid}")]
    public async Task<IActionResult> Detalhes(Guid id)
    {
        var resultado = await clienteAppService.SelecionarPorId(id);
        if (resultado.IsFailed)
            return this.RedirecionarParaNotificacaoHome(resultado.ToResult());

        var cliente = resultado.Value;
        
        var detalhesVm = DetalhesClienteViewModel.ParaDetalhesVm(resultado.Value);

        return View(detalhesVm);
    }
}