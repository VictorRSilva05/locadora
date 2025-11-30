using Locadora.Dominio.Compartilhado;
using Locadora.Dominio.ModuloGrupoVeiculo;

namespace Locadora.Dominio.ModuloVeiculo;
public interface IRepositorioVeiculo : IRepositorio<Veiculo>
{
    Task<List<Veiculo>> FiltrarPorGrupo(GrupoVeiculo grupoVeiculo);
}