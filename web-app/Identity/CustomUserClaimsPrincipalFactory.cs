namespace Locadora.WebApp.Identity;

using Locadora.Dominio.Autenticacao;
using Locadora.Dominio.ModuloFuncionario;
using Locadora.Infraestrutura.Compartilhado;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

// Adicione os usings do seu domínio e EF Core

public class CustomUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<User, Role>
{
    private readonly AppDbContext _dbContext;

    public CustomUserClaimsPrincipalFactory(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        IOptions<IdentityOptions> optionsAccessor,
        AppDbContext dbContext)
        : base(userManager, roleManager, optionsAccessor)
    {
        _dbContext = dbContext;
    }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
    {
        // 1. Gera as claims padrões (Name, Email, Role, Sub, etc.)
        var identity = await base.GenerateClaimsAsync(user);

        // 2. Verifica se o usuário é um funcionário
        // Nota: Assumindo que você tem uma navegação ou busca eficiente. 
        // Se a tabela de funcionários for muito grande, garanta que há índices no UsuarioId.
        var dadosFuncionario = _dbContext.Set<Funcionario>()
            .Where(f => f.UserId == user.Id)
            .Select(f => new { f.EmpresaId })
            .FirstOrDefault();

        if (dadosFuncionario != null)
        {
            // CENÁRIO FUNCIONÁRIO:
            // O Tenant é a empresa para a qual ele trabalha.
            identity.AddClaim(new Claim("EmpresaId", dadosFuncionario.EmpresaId.ToString()));
        }
        else
        {
            // CENÁRIO EMPRESA:
            // Assumindo que se não é funcionário, o próprio usuário é o Tenant (Empresa).
            // Ou você pode verificar explicitamente a Role "Empresa".
            if (await UserManager.IsInRoleAsync(user, "Empresa"))
            {
                identity.AddClaim(new Claim("EmpresaId", user.Id.ToString()));
            }
        }

        return identity;
    }
}