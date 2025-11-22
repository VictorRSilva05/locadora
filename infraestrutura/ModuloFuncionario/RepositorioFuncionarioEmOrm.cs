using Locadora.Dominio.ModuloFuncionario;
using Locadora.Infraestrutura.Compartilhado;

namespace Locadora.Infraestrutura.ModuloFuncionario;
public class RepositorioFuncionarioEmOrm : RepositorioBaseEmOrm<Funcionario>, IRepositorioFuncionario
{
    public RepositorioFuncionarioEmOrm(AppDbContext contexto) : base(contexto)
    {
    }
}
