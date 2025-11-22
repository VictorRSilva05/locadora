using Locadora.Dominio.ModuloCombustivel;
using Locadora.Infraestrutura.Compartilhado;

namespace Locadora.Infraestrutura.ModuloCombustivel;
public class RepositorioCombustivelEmOrm : RepositorioBaseEmOrm<Combustivel>, IRepositorioCombustivel
{
    public RepositorioCombustivelEmOrm(AppDbContext contexto) : base(contexto)
    {
    }
}
