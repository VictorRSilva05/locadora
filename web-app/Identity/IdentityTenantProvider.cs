using Locadora.Dominio.Autenticacao;
using System.Security.Claims;

namespace Locadora.WebApp.Identity;

public sealed class IdentityTenantProvider(IHttpContextAccessor contextAccessor) : ITenantProvider
{
    public Guid? TenantId
    {
        get
        {
            ClaimsPrincipal? claimsPrincipal = contextAccessor.HttpContext?.User;

            if (claimsPrincipal?.Identity?.IsAuthenticated != true)
            {
                return null;
            }

            Claim? claimId = claimsPrincipal.FindFirst("sub");

            if (claimId == null)
            {
                return null;
            }

            return TryParseGuid(claimId.Value);
        }
    }

    public bool IsInRole(string roleName) => contextAccessor.HttpContext?.User?.IsInRole(roleName) ?? false;

    private static Guid? TryParseGuid(string? value)
    {
        if (Guid.TryParse(value, out Guid guid))
        {
            return guid;
        }

        return null;
    }
}