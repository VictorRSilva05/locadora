using Locadora.Dominio.ModuloCombustivel;
using System.ComponentModel.DataAnnotations;

namespace Locadora.WebApp.Models;

public abstract class FormularioCombustivelViewModel
{
    [Required(ErrorMessage = "O campo Nome é obrigatório.")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "O campo Preço é obrigatório.")]
    public decimal Preco { get; set; }

    public static Combustivel ParaEntidade (FormularioCombustivelViewModel viewModel)
    {
        return new Combustivel(viewModel.Nome, viewModel.Preco);
    }
}

public class CadastrarCombustivelViewModel : FormularioCombustivelViewModel
{
    public CadastrarCombustivelViewModel() { }
}

public class  EditarCombustivelViewModel : FormularioCombustivelViewModel
{
    public Guid Id { get; set; }

    public EditarCombustivelViewModel() { }

    public EditarCombustivelViewModel(Guid id, string nome, decimal preco) : this()
    {
        Id = id;
        Nome = nome;
        Preco = preco;
    }
}

public class ExcluirCombustivelViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public ExcluirCombustivelViewModel() { }
    public ExcluirCombustivelViewModel(Guid id, string nome) : this()
    {
        Id = id;
        Nome = nome;
    }
}

public class VisualizarCombustivelViewModel
{
    public List<DetalhesCombustivelViewModel> Registros { get; set; }
    public VisualizarCombustivelViewModel(List<Combustivel> registros)
    {
        Registros = registros
            .Select(DetalhesCombustivelViewModel.ParaDetalhesVm)
            .ToList();
    }
}

public class DetalhesCombustivelViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public decimal Preco { get; set; }
    public static DetalhesCombustivelViewModel ParaDetalhesVm(Combustivel combustivel)
    {
        return new DetalhesCombustivelViewModel
        {
            Id = combustivel.Id,
            Nome = combustivel.Nome,
            Preco = combustivel.Preco
        };
    }
}
