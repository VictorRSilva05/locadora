using Locadora.Aplicacao.ModuloAluguel;
using Locadora.Aplicacao.ModuloCliente;
using Locadora.Aplicacao.ModuloCobranca;
using Locadora.Aplicacao.ModuloCondutor;
using Locadora.Aplicacao.ModuloTaxa;
using Locadora.Aplicacao.ModuloVeiculo;
using Locadora.WebApp.Extensions;
using Locadora.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace Locadora.WebApp.Controllers;

[Route("alugueis")]
public class AluguelController : Controller
{
    private readonly AluguelAppService aluguelAppService;
    private readonly CondutorAppService condutorAppService;
    private readonly ClienteAppService clienteAppService;
    private readonly CobrancaAppService cobrancaAppService;
    private readonly VeiculoAppService veiculoAppService;
    private readonly TaxaAppService taxaAppService;

    public AluguelController(
        AluguelAppService aluguelAppService,
        CondutorAppService condutorAppService,
        ClienteAppService clienteAppService,
        CobrancaAppService cobrancaAppService,
        VeiculoAppService veiculoAppService,
        TaxaAppService taxaAppService
        )
    {
        this.aluguelAppService = aluguelAppService;
        this.condutorAppService = condutorAppService;
        this.clienteAppService = clienteAppService;
        this.cobrancaAppService = cobrancaAppService;
        this.veiculoAppService = veiculoAppService;
        this.taxaAppService = taxaAppService;
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

        var entidade = FormularioAluguelViewModel.ParaEntidade(
            cadastrarVM,
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

    //[HttpGet("editar/{id:guid}")]
    //public async Task<IActionResult> Editar(Guid id)
    //{
    //    var condutores = await condutorAppService.SelecionarTodos();
    //    var clientes = await clienteAppService.SelecionarTodos();
    //    var veiculos = await veiculoAppService.SelecionarTodos();
    //    var cobranca = await cobrancaAppService.SelecionarTodos();
    //    var taxas = await taxaAppService.SelecionarTodos();

    //    var aluguelResultado = await aluguelAppService.SelecionarPorId(id);

    //    if (aluguelResultado.IsFailed)
    //        return this.RedirecionarParaNotificacaoHome(aluguelResultado.ToResult());

    //    var editarVM = new EditarAluguelViewModel(
    //        aluguelResultado.Value,
    //        condutores.ValueOrDefault,
    //        clientes.ValueOrDefault,
    //        veiculos.ValueOrDefault,
    //        cobranca.ValueOrDefault,
    //        taxas.ValueOrDefault
    //        );

    //    return View( editarVM );
    //}

    //[HttpPost("editar/{id:guid}")]
    //public async Task<IActionResult> Editar(Guid id, EditarAluguelViewModel editarVM)
    //{
    //    var condutores = await condutorAppService.SelecionarTodos();
    //    var clientes = await clienteAppService.SelecionarTodos();
    //    var veiculos = await veiculoAppService.SelecionarTodos();
    //    var cobranca = await cobrancaAppService.SelecionarTodos();
    //    var taxas = await taxaAppService.SelecionarTodos();

    //    var entidade = FormularioAluguelViewModel.ParaEntidade(
    //        editarVM,
    //        condutores.ValueOrDefault,
    //        clientes.ValueOrDefault,
    //        veiculos.ValueOrDefault,
    //        cobranca.ValueOrDefault,
    //        taxas.ValueOrDefault
    //        );

    //    entidade.Id = editarVM.Id;

    //    var resultado = await aluguelAppService.Editar(id, entidade);

    //    if (resultado.IsFailed)
    //        return this.PreencherErrosModelState(resultado, editarVM);

    //    return RedirectToAction(nameof(Index));
    //}

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
