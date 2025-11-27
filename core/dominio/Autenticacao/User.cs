using Locadora.Dominio.ModuloFuncionario;
using Microsoft.AspNetCore.Identity;

namespace Locadora.Dominio.Autenticacao;
public class User : IdentityUser<Guid>
{
    public string FullName { get; set; } = string.Empty;

    public User()
    {
        this.Id = Guid.NewGuid();
        this.EmailConfirmed = true;
    }
}