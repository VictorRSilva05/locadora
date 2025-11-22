using Locadora.Dominio.ModuloCondutor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Locadora.Infraestrutura.ModuloCondutor;
public class MapeadorCondutorEmOrm : IEntityTypeConfiguration<Condutor>
{
    public void Configure(EntityTypeBuilder<Condutor> builder)
    {
        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(x => x.Nome)
            .IsRequired();

        builder.Property(x => x.Email)
            .IsRequired();

        builder.Property(x => x.Telefone)
            .IsRequired();

        builder.Property(x => x.Cpf)
            .IsRequired();

        builder.Property(x => x.Cnh)
            .IsRequired();

        builder.Property(x => x.Validade)
            .IsRequired();
    }
}
