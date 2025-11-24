using Locadora.Dominio.ModuloCobranca;
using Locadora.Infraestrutura.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace Locadora.Infraestrutura.ModuloCobranca;
public class RepositorioCobrancaEmOrm : RepositorioBaseEmOrm<Cobranca>, IRepositorioCobranca
{
    public RepositorioCobrancaEmOrm(AppDbContext contexto) : base(contexto)
    {
    }

    public override Task<Cobranca?> SelecionarRegistroPorIdAsync(Guid idRegistro)
    {
        return registros
            .Include(c => c.GrupoVeiculo)
            .FirstOrDefaultAsync(c => c.Id == idRegistro);
    }

    public override Task<List<Cobranca>> SelecionarRegistrosAsync()
    {
        return registros
            .Include(c => c.GrupoVeiculo)
            .ToListAsync();
    }
}
