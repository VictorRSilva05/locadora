using Locadora.Dominio.ModuloTaxa;
using Locadora.Infraestrutura.Compartilhado;

namespace Locadora.Infraestrutura.ModuloTaxa;
public class RepositorioTaxaEmOrm : RepositorioBaseEmOrm<Taxa>, IRepositorioTaxa
{
    public RepositorioTaxaEmOrm(AppDbContext contexto) : base(contexto)
    {
    }
}