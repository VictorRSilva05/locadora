using Locadora.Dominio.ModuloGrupoVeiculo;
using System.ComponentModel.DataAnnotations;

namespace Locadora.WebApp.Models;

public abstract class FormularioGrupoVeiculoViewModel
{
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    public string Nome { get; set; }

    public static GrupoVeiculo ParaEntidade(FormularioGrupoVeiculoViewModel vm)
    {
        return new GrupoVeiculo(vm.Nome);
    }

    public class CadastrarGrupoVeiculoViewModel : FormularioGrupoVeiculoViewModel
    {
        public CadastrarGrupoVeiculoViewModel() { }
    }

    public class EditarGrupoVeiculoViewModel : FormularioGrupoVeiculoViewModel
    {
        public Guid Id { get; set; }
        public EditarGrupoVeiculoViewModel() { }

        public EditarGrupoVeiculoViewModel(Guid id, string nome)
        {
            Id = id;
            Nome = nome;
        }
    }

    public class ExcluirGrupoVeiculoViewModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public ExcluirGrupoVeiculoViewModel() { }
        public ExcluirGrupoVeiculoViewModel(Guid id, string nome)
        {
            Id = id;
            Nome = nome;
        }
    }

    public class VisualizarGrupoVeiculoViewModel
    {
        public List<DetalhesGrupoVeiculoViewModel> Registros { get; set; }

        public VisualizarGrupoVeiculoViewModel(List<GrupoVeiculo> registros)
        {
            Registros = registros
                .Select(DetalhesGrupoVeiculoViewModel.ParaDetalhesVm)
                .ToList();
        }
    }

    public class DetalhesGrupoVeiculoViewModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }

        public DetalhesGrupoVeiculoViewModel(Guid id, string nome)
        {
            Id = id;
            Nome = nome;
        }

        public static DetalhesGrupoVeiculoViewModel ParaDetalhesVm(GrupoVeiculo grupoVeiculo)
        {
            return new DetalhesGrupoVeiculoViewModel(grupoVeiculo.Id, grupoVeiculo.Nome);
        }
    }
}
