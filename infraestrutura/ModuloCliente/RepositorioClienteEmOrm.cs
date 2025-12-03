using Locadora.Dominio.ModuloCliente;
using Locadora.Dominio.ModuloVeiculo;
using Locadora.Infraestrutura.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace Locadora.Infraestrutura.ModuloCliente;
public class RepositorioClienteEmOrm : RepositorioBaseEmOrm<Cliente>, IRepositorioCliente
{
    public RepositorioClienteEmOrm(AppDbContext contexto) : base(contexto)
    {
    }

    public override Task<Cliente?> SelecionarRegistroPorIdAsync(Guid idRegistro)
    {
        return registros
            .Include(v => v.PJ)
            .FirstOrDefaultAsync(v => v.Id == idRegistro);
    }

    public override Task<List<Cliente>> SelecionarRegistrosAsync()
    {
        return registros
            .Include(v => v.PJ)
            .ToListAsync();
    }
}
