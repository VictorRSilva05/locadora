using Locadora.Dominio.ModuloAluguel;
using Locadora.Infraestrutura.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace Locadora.Infraestrutura.ModuloAluguel;
public class RepositorioAluguelEmOrm : RepositorioBaseEmOrm<Aluguel>, IRepositorioAluguel
{
    public RepositorioAluguelEmOrm(AppDbContext contexto) : base(contexto)
    {
    }

    public override Task<Aluguel?> SelecionarRegistroPorIdAsync(Guid idRegistro)
    {
        return registros
            .Include(c => c.Condutor)
            .Include(c => c.Cliente)
            .Include(c => c.Cobranca)
            .Include(c => c.Veiculo)
            .Include(c => c.Veiculo.Combustivel)
            .Include(c => c.Taxas)
            .FirstOrDefaultAsync(c => c.Id == idRegistro);
    }

    public override Task<List<Aluguel>> SelecionarRegistrosAsync()
    {
        return registros
           .Include(c => c.Condutor)
           .Include(c => c.Cliente)
           .Include(c => c.Cobranca)
           .Include(c => c.Veiculo)
           .Include(c => c.Veiculo.Combustivel)
           .Include(c => c.Taxas)
           .ToListAsync();
    }
}
