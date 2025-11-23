using Locadora.Dominio.ModuloCliente;
using Locadora.Infraestrutura.Compartilhado;

namespace Locadora.Infraestrutura.ModuloCliente;
public class RepositorioClienteEmOrm : RepositorioBaseEmOrm<Cliente>, IRepositorioCliente
{
    public RepositorioClienteEmOrm(AppDbContext contexto) : base(contexto)
    {
    }
}
