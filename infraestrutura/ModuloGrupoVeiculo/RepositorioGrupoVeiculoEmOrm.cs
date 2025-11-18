using Locadora.Dominio.ModuloGrupoVeiculo;
using Locadora.Infraestrutura.Compartilhado;

namespace Locadora.Infraestrutura.ModuloGrupoVeiculo;
public class RepositorioGrupoVeiculoEmOrm : RepositorioBaseEmOrm<GrupoVeiculo>, IRepositorioGrupoVeiculo
{
    public RepositorioGrupoVeiculoEmOrm(AppDbContext contexto) : base(contexto) { }
}
