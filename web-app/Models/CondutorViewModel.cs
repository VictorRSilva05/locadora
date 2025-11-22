using Locadora.Dominio.ModuloCondutor;
using System.ComponentModel.DataAnnotations;

namespace Locadora.WebApp.Models;

public abstract class FormularioCondutorViewModel
{
    [Required(ErrorMessage = "O campo Nome é obrigatório.")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "O campo Email é obrigatório.")]
    [EmailAddress(ErrorMessage = "O campo Email deve ser um e-mail válido.")]
    public string Email { get; set; }
    [Required(ErrorMessage = "O campo Telefone é obrigatório.")]
    public string Telefone { get; set; }
    [Required(ErrorMessage = "O campo CPF é obrigatório.")]
    public string Cpf { get; set; }
    [Required(ErrorMessage = "O campo CNH é obrigatório.")]
    public string Cnh { get; set; }
    [Required(ErrorMessage = "O campo Validade é obrigatório.")]
    public DateTime Validade { get; set; }

    public static Condutor ParaEntidade(FormularioCondutorViewModel viewModel)
    {
        return new Condutor(
            viewModel.Nome,
            viewModel.Email,
            viewModel.Telefone,
            viewModel.Cpf,
            viewModel.Cnh,
            viewModel.Validade
        );
    }
}

public class CadastrarCondutorViewModel : FormularioCondutorViewModel
{
    public CadastrarCondutorViewModel() { }
}

public class EditarCondutorViewModel : FormularioCondutorViewModel
{
    public Guid Id { get; set; }
    public EditarCondutorViewModel() { }
    public EditarCondutorViewModel(
        Guid id,
        string nome,
        string email,
        string telefone,
        string cpf,
        string cnh,
        DateTime validade) : this()
    {
        Id = id;
        Nome = nome;
        Email = email;
        Telefone = telefone;
        Cpf = cpf;
        Cnh = cnh;
        Validade = validade;
    }
}

public class ExcluirCondutorViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public ExcluirCondutorViewModel() { }
    public ExcluirCondutorViewModel(Guid id, string nome) : this()
    {
        Id = id;
        Nome = nome;
    }
}

public class VisualizarCondutorViewModel
{
    public List<DetalhesCondutorViewModel> Registros { get; set; } 

    public VisualizarCondutorViewModel(List<Condutor> registros) 
    {
        Registros = registros
            .Select(DetalhesCondutorViewModel.ParaDetalhesVm)
            .ToList();
    }
}

public class DetalhesCondutorViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
    public string Cpf { get; set; }
    public string Cnh { get; set; }
    public DateTime Validade { get; set; }
    public DetalhesCondutorViewModel() { }
    public DetalhesCondutorViewModel(
        Guid id,
        string nome,
        string email,
        string telefone,
        string cpf,
        string cnh,
        DateTime validade)
    {
        Id = id;
        Nome = nome;
        Email = email;
        Telefone = telefone;
        Cpf = cpf;
        Cnh = cnh;
        Validade = validade;
    }

    public static DetalhesCondutorViewModel ParaDetalhesVm(Condutor condutor)
    {
        return new DetalhesCondutorViewModel(
            condutor.Id,
            condutor.Nome,
            condutor.Email,
            condutor.Telefone,
            condutor.Cpf,
            condutor.Cnh,
            condutor.Validade
        );
    }
}
