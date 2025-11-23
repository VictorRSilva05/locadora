using Locadora.Dominio.ModuloTaxa;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Locadora.Infraestrutura.ModuloTaxa;
public class MapeadorTaxaEmOrm : IEntityTypeConfiguration<Taxa>
{
    public void Configure(EntityTypeBuilder<Taxa> builder)
    {
        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(x => x.Descricao)
            .IsRequired();

        builder.Property(x => x.Valor)
            .IsRequired();

        builder.Property(x => x.PlanoCobranca)
            .IsRequired();
    }
}
