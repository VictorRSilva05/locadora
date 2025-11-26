using Microsoft.AspNetCore.Identity;

namespace Locadora.Dominio.Autenticacao;
public class Role : IdentityRole<Guid>;

public enum Roles
{
    Admin,
    Employee
}