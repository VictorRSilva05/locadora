using Locadora.Dominio.ModuloTaxa;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Locadora.WebApp.Models;

public abstract class FormularioTaxaViewModel
{
    [Required(ErrorMessage = "O campo Descrição é obrigatório.")]
    public string Descricao { get; set; }

    [Required(ErrorMessage = "O campo Valor é obrigatório.")]
    public decimal Valor { get; set; }
    [Required(ErrorMessage = "O campo Plano de Cobrança é obrigatório.")]
    public PlanoCobrancaEnum PlanoCobranca { get; set; }

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

    public static Taxa ParaEntidade(FormularioTaxaViewModel viewModel)
    {
        return new Taxa(
            viewModel.Descricao,
            viewModel.Valor,
            viewModel.PlanoCobranca
        );
    }
}

public class CadastrarTaxaViewModel : FormularioTaxaViewModel
{
    public CadastrarTaxaViewModel()
    {
        CarregarPlanosCobranca();
    }
}

public class EditarTaxaViewModel : FormularioTaxaViewModel
{
    public Guid Id { get; set; }
    public EditarTaxaViewModel()
    {
        CarregarPlanosCobranca();
    }
    public EditarTaxaViewModel(Guid id, string descricao, decimal valor, PlanoCobrancaEnum planoCobranca)
    {
        Id = id;
        Descricao = descricao;
        Valor = valor;
        PlanoCobranca = planoCobranca;
    }
}

public class ExcluirTaxaViewModel
{
    public Guid Id { get; set; }
    public string Descricao { get; set; }

    public ExcluirTaxaViewModel() { }

    public ExcluirTaxaViewModel(Guid Id, string Descricao) : this()
    {
        this.Id = Id;
        this.Descricao = Descricao;
    }
}

public class VisualizarTaxaViewModel
{
    public List<DetalhesTaxaViewModel> Registros { get; set; }

    public VisualizarTaxaViewModel(List<Taxa> registros)
    {
        Registros = registros
            .Select(taxa => DetalhesTaxaViewModel.ParaDetalhesVm(taxa))
            .ToList();
    }
}

public class DetalhesTaxaViewModel
{
    public Guid Id { get; set; }
    public string Descricao { get; set; }
    public decimal Valor { get; set; }
    public PlanoCobrancaEnum PlanoCobranca { get; set; }

    public static DetalhesTaxaViewModel ParaDetalhesVm(Taxa taxa)
    {
        return new DetalhesTaxaViewModel
        {
            Id = taxa.Id,
            Descricao = taxa.Descricao,
            Valor = taxa.Valor,
            PlanoCobranca = taxa.PlanoCobranca
        };
    }
}