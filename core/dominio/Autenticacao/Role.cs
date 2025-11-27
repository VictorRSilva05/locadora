using Microsoft.AspNetCore.Identity;

namespace Locadora.Dominio.Autenticacao;
public class Role : IdentityRole<Guid>
{
    public Role() : base() { }

    public Role(string name) : base(name) { }
}

public enum Roles
{
    Admin,
    Employee
}