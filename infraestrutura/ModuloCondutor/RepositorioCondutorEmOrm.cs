using Locadora.Dominio.ModuloCondutor;
using Locadora.Infraestrutura.Compartilhado;

namespace Locadora.Infraestrutura.ModuloCondutor;
public class RepositorioCondutorEmOrm : RepositorioBaseEmOrm<Condutor>, IRepositorioCondutor
{
    public RepositorioCondutorEmOrm(AppDbContext contexto) : base(contexto) { }
}
