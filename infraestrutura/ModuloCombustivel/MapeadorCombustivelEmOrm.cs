using Locadora.Dominio.ModuloCombustivel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Locadora.Infraestrutura.ModuloCombustivel;
public class MapeadorCombustivelEmOrm : IEntityTypeConfiguration<Combustivel>
{
    public void Configure(EntityTypeBuilder<Combustivel> builder)
    {
        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(x => x.Nome)
            .IsRequired();

        builder.Property(x => x.Preco)
            .IsRequired();
    }
}
