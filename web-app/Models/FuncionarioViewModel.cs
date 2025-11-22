using Locadora.Dominio.ModuloFuncionario;
using System.ComponentModel.DataAnnotations;

namespace Locadora.WebApp.Models;

public abstract class FormularioFuncionarioViewModel
{
    [Required(ErrorMessage = "O campo Nome é obrigatório.")]
    public string Nome { get; set; }
    [Required(ErrorMessage = "O campo Data de Admissão é obrigatório.")]
    public DateTime DataAdmissao { get; set; }
    [Required(ErrorMessage = "O campo Salário é obrigatório.")]
    public decimal Salario { get; set; }

    public static Funcionario ParaEntidade(FormularioFuncionarioViewModel viewModel)
    {
        return new Funcionario(
            viewModel.Nome,
            viewModel.DataAdmissao,
            viewModel.Salario
        );
    }
}

public class CadastrarFuncionarioViewModel : FormularioFuncionarioViewModel
{
    public CadastrarFuncionarioViewModel() { }
}

public class EditarFuncionarioViewModel : FormularioFuncionarioViewModel
{
    public Guid Id { get; set; }
    public EditarFuncionarioViewModel() { }
    public EditarFuncionarioViewModel(
        Guid id,
        string nome,
        DateTime dataAdmissao,
        decimal salario) : this()
    {
        Id = id;
        Nome = nome;
        DataAdmissao = dataAdmissao;
        Salario = salario;
    }
}

public class ExcluirFuncionarioViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public ExcluirFuncionarioViewModel() { }
    public ExcluirFuncionarioViewModel(Guid id, string nome) : this()
    {
        Id = id;
        Nome = nome;
    }
}

public class VisualizarFuncionarioViewModel
{
    public List<DetalhesFuncionarioViewModel> Registros { get; set; }
    public VisualizarFuncionarioViewModel(List<Funcionario> registros)
    {
        Registros = registros
            .Select(DetalhesFuncionarioViewModel.ParaDetalhesVm)
            .ToList();
    }
}

public class DetalhesFuncionarioViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public DateTime DataAdmissao { get; set; }
    public decimal Salario { get; set; }

    public DetalhesFuncionarioViewModel(
        Guid id,
        string nome,
        DateTime dataAdmissao,
        decimal salario)
    {
        Id = id;
        Nome = nome;
        DataAdmissao = dataAdmissao;
        Salario = salario;
    }
    public static DetalhesFuncionarioViewModel ParaDetalhesVm(Funcionario funcionario)
    {
        return new DetalhesFuncionarioViewModel(funcionario.Id, funcionario.Nome, funcionario.DataAdmissao, funcionario.Salario);
    }
}
