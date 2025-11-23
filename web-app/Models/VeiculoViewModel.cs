using Locadora.Dominio.ModuloCombustivel;
using Locadora.Dominio.ModuloGrupoVeiculo;
using Locadora.Dominio.ModuloVeiculo;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Locadora.WebApp.Models;

public abstract class FormularioVeiculoViewModel
{
    public IFormFile? FotoUpload { get; set; }

    [Required(ErrorMessage = "O campo Grupo de Veículo é obrigatório.")]
    public Guid GrupoVeiculo { get; set; }

    [Required(ErrorMessage = "O campo Marca é obrigatório.")]
    public string Marca { get; set; }

    [Required(ErrorMessage = "O campo Modelo é obrigatório.")]
    public string Modelo { get; set; }

    [Required(ErrorMessage = "O campo Cor é obrigatório.")]
    public string Cor { get; set; }

    [Required(ErrorMessage = "O campo Combustível é obrigatório.")]
    public Guid Combustivel { get; set; }

    [Required(ErrorMessage = "O campo Capacidade do Tanque é obrigatório.")]
    public int CapacidadeCombustivel { get; set; }

    [Required(ErrorMessage = "O campo Ano é obrigatório.")]
    public int Ano { get; set; }

    public List<SelectListItem>? GrupoVeiculosDisponiveis { get; set; }
    public List<SelectListItem>? CombustiveisDisponiveis { get; set; }

    public static Veiculo ParaEntidade(
        FormularioVeiculoViewModel viewModel,
        List<GrupoVeiculo> grupoVeiculo,
        List<Combustivel> combustivel)
    {
        var grupoVeiculosSelecionado = grupoVeiculo.Find(gv => gv.Id == viewModel.GrupoVeiculo)
            ?? throw new ArgumentException("Grupo de Veículo inválido.");

        var combustivelSelecionado = combustivel.Find(c => c.Id == viewModel.Combustivel)
            ?? throw new ArgumentException("Combustível inválido.");

        byte[]? fotoBytes = null;

        if (viewModel.FotoUpload != null)
        {
            using var ms = new MemoryStream();
            viewModel.FotoUpload.CopyTo(ms);
            fotoBytes = ms.ToArray();
        }

        return new Veiculo(
            fotoBytes,
            grupoVeiculosSelecionado,
            viewModel.Marca,
            viewModel.Modelo,
            viewModel.Cor,
            combustivelSelecionado,
            viewModel.CapacidadeCombustivel,
            viewModel.Ano
        );
    }
}


public class CadastrarVeiculoViewModel : FormularioVeiculoViewModel
{
    public CadastrarVeiculoViewModel() { }

    public CadastrarVeiculoViewModel(List<GrupoVeiculo> grupoVeiculos, List<Combustivel> combustiveis)
    {
        GrupoVeiculosDisponiveis = grupoVeiculos
            .Select(gv => new SelectListItem(gv.Nome, gv.Id.ToString()))
            .ToList();

        CombustiveisDisponiveis = combustiveis
            .Select(c => new SelectListItem(c.Nome, c.Id.ToString()))
            .ToList();
    }
}


public class EditarVeiculoViewModel : FormularioVeiculoViewModel
{
    public Guid Id { get; set; }

    public string? FotoAtualBase64 { get; set; } // para exibir a foto existente

    public EditarVeiculoViewModel() { }

    public EditarVeiculoViewModel(
        Veiculo veiculo,
        List<GrupoVeiculo> grupos,
        List<Combustivel> combustiveis)
    {
        Id = veiculo.Id;
        Marca = veiculo.Marca;
        Modelo = veiculo.Modelo;
        Cor = veiculo.Cor;
        Ano = veiculo.Ano;
        GrupoVeiculo = veiculo.GrupoVeiculo.Id;
        Combustivel = veiculo.Combustivel.Id;
        CapacidadeCombustivel = veiculo.CapacidadeCombustivel;

        FotoAtualBase64 = veiculo.Foto != null
            ? Convert.ToBase64String(veiculo.Foto)
            : null;

        GrupoVeiculosDisponiveis = grupos
            .Select(gv => new SelectListItem(gv.Nome, gv.Id.ToString()))
            .ToList();

        CombustiveisDisponiveis = combustiveis
            .Select(c => new SelectListItem(c.Nome, c.Id.ToString()))
            .ToList();
    }
}


public class ExcluirVeiculoViewModel
{
    public Guid Id { get; set; }
    public string Modelo { get; set; }

    public ExcluirVeiculoViewModel() { }

    public ExcluirVeiculoViewModel(Guid id, string modelo)
    {
        Id = id;
        Modelo = modelo;
    }
}


public class VisualizarVeiculoViewModel
{
    public List<DetalhesVeiculoViewModel> Registros { get; set; }

    public VisualizarVeiculoViewModel(List<Veiculo> registros)
    {
        Registros = registros
            .Select(DetalhesVeiculoViewModel.ParaDetalhesVm)
            .ToList();
    }
}


public class DetalhesVeiculoViewModel
{
    public Guid Id { get; set; }
    public string? FotoBase64 { get; set; }
    public string GrupoVeiculo { get; set; }
    public string Marca { get; set; }
    public string Modelo { get; set; }
    public string Cor { get; set; }
    public string Combustivel { get; set; }
    public int CapacidadeCombustivel { get; set; }
    public int Ano { get; set; }

    public static DetalhesVeiculoViewModel ParaDetalhesVm(Veiculo veiculo)
    {
        return new DetalhesVeiculoViewModel
        {
            Id = veiculo.Id,

            FotoBase64 = veiculo.Foto != null
                ? Convert.ToBase64String(veiculo.Foto)
                : null,

            GrupoVeiculo = veiculo.GrupoVeiculo.Nome,
            Marca = veiculo.Marca,
            Modelo = veiculo.Modelo,
            Cor = veiculo.Cor,
            Combustivel = veiculo.Combustivel.Nome,
            CapacidadeCombustivel = veiculo.CapacidadeCombustivel,
            Ano = veiculo.Ano
        };
    }
}


