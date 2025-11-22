using Locadora.Dominio.Compartilhado;
using Locadora.Dominio.ModuloCombustivel;
using Locadora.Dominio.ModuloFuncionario;
using Locadora.Dominio.ModuloGrupoVeiculo;
using Microsoft.EntityFrameworkCore;

namespace Locadora.Infraestrutura.Compartilhado;
public class AppDbContext : DbContext, IUnitOfWork
{
    public DbSet<GrupoVeiculo> grupoVeiculos { get; set; }
    public DbSet<Combustivel> combustivels { get; set; }
    public DbSet<Funcionario> funcionarios { get; set; }
    public AppDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var assembly = typeof(AppDbContext).Assembly;

        modelBuilder.ApplyConfigurationsFromAssembly(assembly);

        base.OnModelCreating(modelBuilder);
    }
    public async Task CommitAsync()
    {
        await SaveChangesAsync();
    }

    public async Task RollbackAsync()
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.State = EntityState.Unchanged;
                    break;

                case EntityState.Modified:
                    entry.State = EntityState.Unchanged;
                    break;

                case EntityState.Deleted:
                    entry.State = EntityState.Unchanged;
                    break;
            }
        }

        await Task.CompletedTask;
    }
}