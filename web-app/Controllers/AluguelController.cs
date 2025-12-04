using Locadora.Aplicacao.ModuloAluguel;
using Locadora.Aplicacao.ModuloCliente;
using Locadora.Aplicacao.ModuloCobranca;
using Locadora.Aplicacao.ModuloCondutor;
using Locadora.Aplicacao.ModuloFuncionario;
using Locadora.Aplicacao.ModuloTaxa;
using Locadora.Aplicacao.ModuloVeiculo;
using Locadora.Dominio.Autenticacao;
using Locadora.WebApp.Extensions;
using Locadora.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Locadora.WebApp.Controllers;

[Route("alugueis")]
[Authorize(Roles = "Admin, Employee")]
public class AluguelController : Controller
{
    private readonly AluguelAppService aluguelAppService;
    private readonly CondutorAppService condutorAppService;
    private readonly ClienteAppService clienteAppService;
    private readonly CobrancaAppService cobrancaAppService;
    private readonly VeiculoAppService veiculoAppService;
    private readonly TaxaAppService taxaAppService;
    private readonly UserManager<User> userManager;
    private readonly FuncionarioAppService funcionarioAppService;

    public AluguelController(
        AluguelAppService aluguelAppService,
        CondutorAppService condutorAppService,
        ClienteAppService clienteAppService,
        CobrancaAppService cobrancaAppService,
        VeiculoAppService veiculoAppService,
        TaxaAppService taxaAppService
,
        UserManager<User> userManager,
        FuncionarioAppService funcionarioAppService)
    {
        this.aluguelAppService = aluguelAppService;
        this.condutorAppService = condutorAppService;
        this.clienteAppService = clienteAppService;
        this.cobrancaAppService = cobrancaAppService;
        this.veiculoAppService = veiculoAppService;
        this.taxaAppService = taxaAppService;
        this.userManager = userManager;
        this.funcionarioAppService = funcionarioAppService;
    }

    [HttpGet("listar")]
    public async Task<IActionResult> Index()
    {
        var resultado = await aluguelAppService.SelecionarTodos();

        if (resultado.IsFailed)
            return this.RedirecionarParaNotificacaoHome(resultado.ToResult());

        var visualizarVM = new VisualizarAluguelViewModel(resultado.Value);

        this.ObterNotificacaoPendente();

        return View(visualizarVM);
    }

    [HttpGet("cadastrar")]
    public async Task<IActionResult> Cadastrar()
    {
        var condutores = await condutorAppService.SelecionarTodos();
        var clientes = await clienteAppService.SelecionarTodos();
        var veiculos = await veiculoAppService.SelecionarTodos();
        var cobranca = await cobrancaAppService.SelecionarTodos();
        var taxas = await taxaAppService.SelecionarTodos();

        var cadastrarVM = new CadastrarAluguelViewModel(
            condutores.ValueOrDefault,
            clientes.ValueOrDefault,
            veiculos.ValueOrDefault,
            cobranca.ValueOrDefault,
            taxas.ValueOrDefault
            );

        return View(cadastrarVM);
    }

    [HttpPost("cadastrar")]
    public async Task<IActionResult> Cadastrar(CadastrarAluguelViewModel cadastrarVM)
    {
        var condutores = await condutorAppService.SelecionarTodos();
        var clientes = await clienteAppService.SelecionarTodos();
        var veiculos = await veiculoAppService.SelecionarTodos();
        var cobranca = await cobrancaAppService.SelecionarTodos();
        var taxas = await taxaAppService.SelecionarTodos();
        var funcionarios = await funcionarioAppService.SelecionarTodos();

        var usuario = await userManager.FindByIdAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        var funcionario = funcionarios.Value.Find(x => x.UserId == usuario.Id);

        var entidade = FormularioAluguelViewModel.ParaEntidade(
            cadastrarVM,
            funcionario,
            condutores.ValueOrDefault,
            clientes.ValueOrDefault,
            veiculos.ValueOrDefault,
            cobranca.ValueOrDefault,
            taxas.ValueOrDefault);

        var resultado = await aluguelAppService.Cadastrar(entidade);

        if (resultado.IsFailed)
            return this.PreencherErrosModelState(resultado, cadastrarVM);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("devolver/{id:guid}")]
    public async Task<IActionResult> Devolver(Guid id)
    {
        var resultado = await aluguelAppService.SelecionarPorId(id);

        if (resultado.IsFailed)
            return this.RedirecionarParaNotificacaoHome(resultado.ToResult());

        var aluguel = resultado.Value;

        var devolverVM = new DevolucaoAluguelViewModel
        {
            Id = aluguel.Id,
            DataDevolucao = DateTime.Now,
            KmDevolucao = aluguel.KmInicial,
            TanqueCheio = false,
            SeguroAcionado = false,
            Total = 0
        };

        return View(devolverVM);
    }

    [HttpPost("devolver/{id:guid}")]
    public async Task<IActionResult> Devolver(Guid id, DevolucaoAluguelViewModel devolverVM)
    {
        var resultadoSelecao = await aluguelAppService.SelecionarPorId(id);

        if (resultadoSelecao.IsFailed)
            return this.RedirecionarParaNotificacaoHome(resultadoSelecao.ToResult());

        var aluguel = resultadoSelecao.Value;

        //aluguel.DataDevolucao = devolverVM.DataDevolucao;
        //aluguel.KmDevolucao = devolverVM.KmDevolucao;
        //aluguel.TanqueCheio = devolverVM.TanqueCheio;
        //aluguel.SeguroAcionado = devolverVM.SeguroAcionado;
        //aluguel.Total = devolverVM.Total;

        var resultado = await aluguelAppService.Editar(id, aluguel);

        if (resultado.IsFailed)
            return this.PreencherErrosModelState(resultado, devolverVM);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("editar/{id:guid}")]
    public async Task<IActionResult> Editar(Guid id)
    {
        var condutores = await condutorAppService.SelecionarTodos();
        var clientes = await clienteAppService.SelecionarTodos();
        var veiculos = await veiculoAppService.SelecionarTodos();
        var cobranca = await cobrancaAppService.SelecionarTodos();
        var taxas = await taxaAppService.SelecionarTodos();

        var aluguelResultado = await aluguelAppService.SelecionarPorId(id);

        if (aluguelResultado.IsFailed)
            return this.RedirecionarParaNotificacaoHome(aluguelResultado.ToResult());

        var editarVM = new EditarAluguelViewModel(
            aluguelResultado.Value,
            condutores.ValueOrDefault,
            clientes.ValueOrDefault,
            veiculos.ValueOrDefault,
            cobranca.ValueOrDefault,
            taxas.ValueOrDefault
            );

        return View(editarVM);
    }

    [HttpPost("editar/{id:guid}")]
    public async Task<IActionResult> Editar(Guid id, EditarAluguelViewModel editarVM)
    {
        var condutores = await condutorAppService.SelecionarTodos();
        var clientes = await clienteAppService.SelecionarTodos();
        var veiculos = await veiculoAppService.SelecionarTodos();
        var cobranca = await cobrancaAppService.SelecionarTodos();
        var taxas = await taxaAppService.SelecionarTodos();
        var funcionarios = await funcionarioAppService.SelecionarTodos();

        var usuario = await userManager.FindByIdAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        var funcionario = funcionarios.Value.Find(x => x.UserId == usuario.Id);

        var entidade = FormularioAluguelViewModel.ParaEntidade(
            editarVM,
            funcionario,
            condutores.ValueOrDefault,
            clientes.ValueOrDefault,
            veiculos.ValueOrDefault,
            cobranca.ValueOrDefault,
            taxas.ValueOrDefault
            );

        entidade.Id = editarVM.Id;

        var resultado = await aluguelAppService.Editar(id, entidade);

        if (resultado.IsFailed)
            return this.PreencherErrosModelState(resultado, editarVM);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("excluir/{id:guid}")]
    public async Task<IActionResult> Excluir(Guid id)
    {
        var aluguelResultado = await aluguelAppService.SelecionarPorId(id);

        if (aluguelResultado.IsFailed)
            return this.RedirecionarParaNotificacaoHome(aluguelResultado.ToResult());

        var excluirVM = new ExcluirAluguelViewModel(aluguelResultado.Value!);

        return View(excluirVM);
    }

    [HttpPost("excluir/{id:guid}")]
    public async Task<IActionResult> Excluir(Guid id, ExcluirAluguelViewModel excluirVM)
    {
        var resultado = await aluguelAppService.Excluir(id);

        if (resultado.IsFailed)
            return this.PreencherErrosModelState(resultado, excluirVM);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("detalhes/{id:guid}")]
    public async Task<IActionResult> Detalhes(Guid id)
    {
        var aluguelResultado = await aluguelAppService.SelecionarPorId(id);

        if (aluguelResultado.IsFailed)
            return this.RedirecionarParaNotificacaoHome(aluguelResultado.ToResult());

        var detalhesVm = DetalhesAluguelViewModel.ParaDetalhesVm(aluguelResultado.Value!);

        return View(detalhesVm);
    }
}
