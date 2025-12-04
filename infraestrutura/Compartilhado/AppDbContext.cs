using FluentResults;
using Locadora.Dominio.Autenticacao;
using Locadora.Dominio.Compartilhado;
using Locadora.Dominio.ModuloAluguel;
using Locadora.Dominio.ModuloCliente;
using Locadora.Dominio.ModuloCobranca;
using Locadora.Dominio.ModuloCombustivel;
using Locadora.Dominio.ModuloCondutor;
using Locadora.Dominio.ModuloFuncionario;
using Locadora.Dominio.ModuloGrupoVeiculo;
using Locadora.Dominio.ModuloTaxa;
using Locadora.Dominio.ModuloVeiculo;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Locadora.Infraestrutura.Compartilhado;
public class AppDbContext : IdentityDbContext<User, Role, Guid>, IUnitOfWork
{
    public DbSet<GrupoVeiculo> grupoVeiculos { get; set; }
    public DbSet<Combustivel> combustivels { get; set; }
    public DbSet<Funcionario> funcionarios { get; set; }
    public DbSet<Condutor> condutores { get; set; }
    public DbSet<Veiculo> veiculos { get; set; }
    public DbSet<Cliente> clientes { get; set; }
    public DbSet<Taxa> taxas { get; set; }
    public DbSet<Cobranca> cobrancas { get; set; }
    public DbSet<Aluguel> aluguel { get; set; }
    public DbSet<Devolucao> devolucoes { get; set; }

    private readonly ITenantProvider? tenantProvider;

    public AppDbContext(
        DbContextOptions<AppDbContext> options,
        ITenantProvider? tenantProvider = null
    ) : base(options)
    {
        this.tenantProvider = tenantProvider;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (tenantProvider is not null)
        {
            modelBuilder.Entity<Funcionario>()
                .HasQueryFilter(x => x.EmpresaId.Equals(tenantProvider.GetTenantId()));
            modelBuilder.Entity<Combustivel>()
                .HasQueryFilter(x => x.EmpresaId.Equals(tenantProvider.GetTenantId()));
            modelBuilder.Entity<GrupoVeiculo>()
                .HasQueryFilter(x => x.EmpresaId.Equals(tenantProvider.GetTenantId()));
            modelBuilder.Entity<Condutor>()
                .HasQueryFilter(x => x.EmpresaId.Equals(tenantProvider.GetTenantId()));
            modelBuilder.Entity<Veiculo>()
                .HasQueryFilter(x => x.EmpresaId.Equals(tenantProvider.GetTenantId()));
            modelBuilder.Entity<Cliente>()
                .HasQueryFilter(x => x.EmpresaId.Equals(tenantProvider.GetTenantId()));
            modelBuilder.Entity<Taxa>()
                .HasQueryFilter(x => x.EmpresaId.Equals(tenantProvider.GetTenantId()));
            modelBuilder.Entity<Cobranca>()
                .HasQueryFilter(x => x.EmpresaId.Equals(tenantProvider.GetTenantId()));
            modelBuilder.Entity<Aluguel>()
                .HasQueryFilter(x => x.EmpresaId.Equals(tenantProvider.GetTenantId()));
            modelBuilder.Entity<Devolucao>()
                .HasQueryFilter(x => x.EmpresaId.Equals(tenantProvider.GetTenantId()));
        }

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Funcionario>()
              .HasOne(f => f.User)
              .WithOne()
              .HasForeignKey<Funcionario>(f => f.UserId)
              .OnDelete(DeleteBehavior.Restrict);
    }

    public async Task CommitAsync() => await SaveChangesAsync();

    public async Task RollbackAsync()
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                case EntityState.Modified:
                case EntityState.Deleted:
                    entry.State = EntityState.Unchanged;
                    break;
            }
        }
        await Task.CompletedTask;
    }
}
