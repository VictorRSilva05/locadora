using Locadora.Dominio.Autenticacao;
using System.ComponentModel.DataAnnotations;

namespace Locadora.WebApp.Models;

public class RegistroViewModel
{
    [Required(ErrorMessage = "O campo \"Email\" é obrigatório.")]
    [EmailAddress(ErrorMessage = "O campo \"Email\" precisa ser um endereço de email válido.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "O campo \"Senha\" é obrigatório.")]
    public string? Senha { get; set; }

    [Required(ErrorMessage = "O campo \"Confirmação da Senha\" é obrigatório.")]
    [Compare(nameof(Senha), ErrorMessage = "O campo \"Confirmação da Senha\" não é igual à senha.")]
    public string? ConfirmarSenha { get; set; }

    [Required(ErrorMessage = "O campo \"Tipo de Usuário\" é obrigatório.")]
    public Roles Tipo { get; set; }

    public RegistroViewModel() { }
}

public class LoginViewModel
{
    [Required(ErrorMessage = "O campo Email é obrigatório.")]
    [EmailAddress(ErrorMessage = "Email inválido.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "A senha é obrigatória.")]
    [DataType(DataType.Password)]
    public string Senha { get; set; }

    public bool LembrarMe { get; set; }
}

public class RegistrarAdminViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Senha { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare("Senha")]
    public string ConfirmarSenha { get; set; }

    public RegistrarAdminViewModel() { }
}
