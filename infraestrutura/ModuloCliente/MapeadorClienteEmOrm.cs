using Locadora.Dominio.ModuloCliente;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Locadora.Infraestrutura.ModuloCliente;
public class MapeadorClienteEmOrm : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(x => x.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Telefone)
            .IsRequired()
            .HasMaxLength(15);

        builder.Property(x => x.TipoCliente)
            .IsRequired();

        builder.Property(x => x.CPF)
            .HasMaxLength(14);

        builder.Property(x => x.CNPJ)
            .HasMaxLength(18);

        builder.Property(x => x.RG)
            .IsRequired();

        builder.Property(x => x.CNH)
            .IsRequired();

        builder.Property(x => x.Estado)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Cidade)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Bairro)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Rua)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Numero)
            .IsRequired()
            .HasMaxLength(10);

        builder.HasOne(x => x.PJ);

        builder.HasMany(x => x.PF);
    }
}
