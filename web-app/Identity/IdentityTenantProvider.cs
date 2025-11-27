using Locadora.Dominio.Autenticacao;
using System.Security.Claims;

namespace Locadora.WebApp.Identity;

public sealed class IdentityTenantProvider(IHttpContextAccessor contextAccessor) : ITenantProvider
{
    public Guid? TenantId
    {
        get
        {
            var claimId = contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);

            if (claimId == null)
                return null;

            return Guid.Parse(claimId.Value);
        }
    }

    public bool IsInRole(string roleName) => contextAccessor.HttpContext?.User?.IsInRole(roleName) ?? false;

}