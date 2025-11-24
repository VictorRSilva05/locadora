using Locadora.Dominio.ModuloCobranca;
using Locadora.Dominio.ModuloGrupoVeiculo;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Locadora.WebApp.Models;

public abstract class FormularioCobrancaViewModel
{
    [Required(ErrorMessage = "O campo Grupo de Veículo é obrigatório.")]
    public Guid GrupoVeiculo { get; set; }
    [Required(ErrorMessage = "O campo Plano de Cobrança é obrigatório.")]
    public PlanoCobrancaEnum PlanoCobranca { get; set; }
    public decimal? PrecoDiaria { get; set; }
    public decimal? PrecoKm { get; set; }
    public int? KmDisponiveis { get; set; }
    public decimal? PrecoPorKmExtrapolado { get; set; }
    public decimal? Taxa { get; set; }

    public List<SelectListItem>? GrupoVeiculosDisponiveis { get; set; }
    public List<SelectListItem>? PlanosCobrancaDisponiveis { get; set; }

    public void CarregarPlanosCobranca()
    {
        PlanosCobrancaDisponiveis = Enum.GetValues(typeof(PlanoCobrancaEnum))
            .Cast<PlanoCobrancaEnum>()
            .Select(plano => new SelectListItem
            {
                Value = ((int)plano).ToString(),
                Text = plano.ToString()
            })
            .ToList();
    }

    public static Cobranca ParaEntidade(
        FormularioCobrancaViewModel viewModel,
        List<GrupoVeiculo> grupoVeiculo)
    {
        var grupoVeiculosSelecionado = grupoVeiculo.Find(gv => gv.Id == viewModel.GrupoVeiculo)
            ?? throw new ArgumentException("Grupo de Veículo inválido.");

        return new Cobranca(
            grupoVeiculosSelecionado,
            viewModel.PlanoCobranca,
            viewModel.PrecoDiaria,
            viewModel.PrecoKm,
            viewModel.KmDisponiveis,
            viewModel.PrecoPorKmExtrapolado,
            viewModel.Taxa
        );
    }
}

public class CadastrarCobrancaViewModel : FormularioCobrancaViewModel
{
    public CadastrarCobrancaViewModel()
    {
        CarregarPlanosCobranca();
    }

    public CadastrarCobrancaViewModel(List<GrupoVeiculo> grupoVeiculos)
    {
        CarregarPlanosCobranca();
        GrupoVeiculosDisponiveis = grupoVeiculos
            .Select(gv => new SelectListItem
            {
                Value = gv.Id.ToString(),
                Text = gv.Nome
            })
            .ToList();
    }
}

public class EditarCobrancaViewModel : FormularioCobrancaViewModel
{
    public Guid Id { get; set; }
    public EditarCobrancaViewModel()
    {
        CarregarPlanosCobranca();
    }
    public EditarCobrancaViewModel(
        Cobranca cobranca,
        List<GrupoVeiculo> grupoVeiculos
        )
    {
        Id = cobranca.Id;
        GrupoVeiculo = cobranca.GrupoVeiculo.Id;
        PlanoCobranca = cobranca.PlanoCobranca;
        PrecoDiaria = cobranca.PrecoDiaria;
        PrecoKm = cobranca.PrecoKm;
        KmDisponiveis = cobranca.KmDisponiveis;
        PrecoPorKmExtrapolado = cobranca.PrecoPorKmExtrapolado;
        Taxa = cobranca.Taxa;

        CarregarPlanosCobranca();

        GrupoVeiculosDisponiveis = grupoVeiculos
            .Select(gv => new SelectListItem
            {
                Value = gv.Id.ToString(),
                Text = gv.Nome
            })
            .ToList();
    }
}

public class ExcluirCobrancaViewModel
{
    public Guid Id { get; set; }
    public string GrupoVeiculo { get; set; }
    public PlanoCobrancaEnum PlanoCobranca { get; set; }
    public ExcluirCobrancaViewModel(
        Cobranca cobranca)
    {
        Id = cobranca.Id;
        GrupoVeiculo = cobranca.GrupoVeiculo.Nome;
        PlanoCobranca = cobranca.PlanoCobranca;
    }
}

public class VisualizarCobrancaViewModel
{
    public List<DetalhesCobrancaViewModel> Cobrancas { get; set; }

    public VisualizarCobrancaViewModel(List<Cobranca> cobrancas)
    {
        Cobrancas = cobrancas
            .Select(c => DetalhesCobrancaViewModel.ParaDetalhesVm(c))
            .ToList();
    }
}   

public class DetalhesCobrancaViewModel
{
    public Guid Id { get; set; }
    public string GrupoVeiculo { get; set; }
    public PlanoCobrancaEnum PlanoCobranca { get; set; }
    public decimal? PrecoDiaria { get; set; }
    public decimal? PrecoKm { get; set; }
    public int? KmDisponiveis { get; set; }
    public decimal? PrecoPorKmExtrapolado { get; set; }
    public decimal? Taxa { get; set; }
    public static DetalhesCobrancaViewModel ParaDetalhesVm(Cobranca cobranca)
    {
        return new DetalhesCobrancaViewModel
        {
            Id = cobranca.Id,
            GrupoVeiculo = cobranca.GrupoVeiculo.Nome,
            PlanoCobranca = cobranca.PlanoCobranca,
            PrecoDiaria = cobranca.PrecoDiaria,
            PrecoKm = cobranca.PrecoKm,
            KmDisponiveis = cobranca.KmDisponiveis,
            PrecoPorKmExtrapolado = cobranca.PrecoPorKmExtrapolado,
            Taxa = cobranca.Taxa
        };
    }
}
