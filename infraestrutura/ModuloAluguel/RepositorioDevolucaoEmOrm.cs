using Locadora.Dominio.ModuloAluguel;
using Locadora.Infraestrutura.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace Locadora.Infraestrutura.ModuloAluguel;
public class RepositorioDevolucaoEmOrm : RepositorioBaseEmOrm<Devolucao>, IRepositorioDevolucao
{
    public RepositorioDevolucaoEmOrm(AppDbContext contexto) : base(contexto)
    {
    }

    public override Task<Devolucao?> SelecionarRegistroPorIdAsync(Guid idRegistro)
    {
        return registros
            .Include(x => x.Aluguel)
            .FirstOrDefaultAsync(x => x.Id == idRegistro);
    }

    public override Task<List<Devolucao>> SelecionarRegistrosAsync()
    {
        return registros
            .Include(x => x.Aluguel)
            .ToListAsync();
              

    }
}
