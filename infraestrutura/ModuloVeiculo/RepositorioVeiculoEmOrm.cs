using Locadora.Dominio.ModuloVeiculo;
using Locadora.Infraestrutura.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace Locadora.Infraestrutura.ModuloVeiculo;
public class RepositorioVeiculoEmOrm : RepositorioBaseEmOrm<Veiculo>, IRepositorioVeiculo
{
    public RepositorioVeiculoEmOrm(AppDbContext contexto) : base(contexto) { }

    public override Task<Veiculo?> SelecionarRegistroPorIdAsync(Guid idRegistro)
    {
        return registros
            .Include(v => v.GrupoVeiculo)
            .Include(v => v.Combustivel)
            .FirstOrDefaultAsync(v => v.Id == idRegistro);
    }

    public override Task<List<Veiculo>> SelecionarRegistrosAsync()
    {
        return registros
            .Include(v => v.GrupoVeiculo)
            .Include(v => v.Combustivel)
            .ToListAsync();
    }
}
