using Locadora.Dominio.ModuloFuncionario;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Locadora.Infraestrutura.ModuloFuncionario;
public class MapeadorFuncionarioEmOrm : IEntityTypeConfiguration<Funcionario>
{
    public void Configure(EntityTypeBuilder<Funcionario> builder)
    {
        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(x => x.Email);

        builder.Property(x => x.Nome)
            .IsRequired();

        builder.Property(x => x.DataAdmissao)
            .IsRequired();

        builder.Property(x => x.Salario)
            .IsRequired();

        builder.HasOne(t => t.Tenant)
          .WithMany()
          .HasForeignKey(t => t.EmpresaId)
          .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(u => u.User)
               .WithOne()
               .HasForeignKey<Funcionario>(e => e.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(f => new { f.UserId, f.EstaAtivo });
    }
}
