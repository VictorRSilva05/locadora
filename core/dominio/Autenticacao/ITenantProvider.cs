namespace Locadora.Dominio.Autenticacao;
public interface ITenantProvider
{
    Guid? TenantId { get; }
    Guid GetTenantId() => this.TenantId.GetValueOrDefault();
}
