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
    public IActionResult Index()
    {
        var gruposVeiculos = grupoVeiculoAppService.SelecionarTodos();
        return View(gruposVeiculos);
    }

    [HttpGet("cadastrar")]
    public IActionResult Cadastrar()
    {
        var cadastrarVm = new CadastrarGrupoVeiculoViewModel();

        return View();
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
