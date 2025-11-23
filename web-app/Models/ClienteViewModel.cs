using Locadora.Dominio.ModuloCliente;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Locadora.WebApp.Models;

public abstract class FormularioClienteViewModel
{
    [Required(ErrorMessage = "O campo Nome é obrigatório.")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "O campo E-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "O campo E-mail deve ser um endereço de e-mail válido.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "O campo Telefone é obrigatório.")]
    public string Telefone { get; set; }

    [Required(ErrorMessage = "O campo Tipo de Cliente é obrigatório.")]
    public TipoClienteEnum TipoCliente { get; set; }

    public string? CPF { get; set; }
    public string? CNPJ { get; set; }

    [Required(ErrorMessage = "O campo Estado é obrigatório.")]
    public string Estado { get; set; }

    [Required(ErrorMessage = "O campo Cidade é obrigatório.")]
    public string Cidade { get; set; }

    [Required(ErrorMessage = "O campo Bairro é obrigatório.")]
    public string Bairro { get; set; }

    [Required(ErrorMessage = "O campo Rua é obrigatório.")]
    public string Rua { get; set; }

    [Required(ErrorMessage = "O campo Número é obrigatório.")]
    public string Numero { get; set; }

    public List<SelectListItem>? TiposClienteDisponiveis { get; set; }

    public void CarregarTiposCliente()
    {
        TiposClienteDisponiveis = Enum.GetValues(typeof(TipoClienteEnum))
            .Cast<TipoClienteEnum>()
            .Select(tipo => new SelectListItem
            {
                Value = ((int)tipo).ToString(),
                Text = tipo.ToString()
            })
            .ToList();
    }

    public static Cliente ParaEntidade(FormularioClienteViewModel viewModel)
    {
        return new Cliente(
            viewModel.Nome,
            viewModel.Email,
            viewModel.Telefone,
            viewModel.TipoCliente,
            viewModel.CPF,
            viewModel.CNPJ,
            viewModel.Estado,
            viewModel.Cidade,
            viewModel.Bairro,
            viewModel.Rua,
            viewModel.Numero
        );
    }
}


public class CadastrarClienteViewModel : FormularioClienteViewModel
{
    public CadastrarClienteViewModel()
    {
        CarregarTiposCliente();
    }

}

public class EditarClienteViewModel : FormularioClienteViewModel
{
    public Guid Id { get; set; }
    public EditarClienteViewModel() { }
    public EditarClienteViewModel(
        Guid id,
        string nome,
        string email,
        string telefone,
        TipoClienteEnum tipoCliente,
        string? cpf,
        string? cnpj,
        string estado,
        string cidade,
        string bairro,
        string rua,
        string numero)
    {
        Id = id;
        Nome = nome;
        Email = email;
        Telefone = telefone;
        TipoCliente = tipoCliente;
        CPF = cpf;
        CNPJ = cnpj;
        Estado = estado;
        Cidade = cidade;
        Bairro = bairro;
        Rua = rua;
        Numero = numero;
    }
}

public class ExcluirClienteViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public ExcluirClienteViewModel() { }
    public ExcluirClienteViewModel(Guid id, string nome) : this()
    {
        Id = id;
        Nome = nome;
    }
}

public class VisualizarClienteViewModel
{
    public List<DetalhesClienteViewModel> Registros { get; set; }
    public VisualizarClienteViewModel(List<Cliente> registros)
    {
        Registros = registros
            .Select(DetalhesClienteViewModel.ParaDetalhesVm)
            .ToList();
    }
}

public class DetalhesClienteViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
    public TipoClienteEnum TipoCliente { get; set; }
    public string? CPF { get; set; }
    public string? CNPJ { get; set; }
    public string Estado { get; set; }
    public string Cidade { get; set; }
    public string Bairro { get; set; }
    public string Rua { get; set; }
    public string Numero { get; set; }
    public static DetalhesClienteViewModel ParaDetalhesVm(Cliente cliente)
    {
        return new DetalhesClienteViewModel
        {
            Id = cliente.Id,
            Nome = cliente.Nome,
            Email = cliente.Email,
            Telefone = cliente.Telefone,
            TipoCliente = cliente.TipoCliente,
            CPF = cliente.CPF,
            CNPJ = cliente.CNPJ,
            Estado = cliente.Estado,
            Cidade = cliente.Cidade,
            Bairro = cliente.Bairro,
            Rua = cliente.Rua,
            Numero = cliente.Numero
        };
    }
}
