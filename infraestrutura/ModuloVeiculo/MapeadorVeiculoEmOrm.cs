using Locadora.Dominio.ModuloVeiculo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Locadora.Infraestrutura.ModuloVeiculo;
public class MapeadorVeiculoEmOrm : IEntityTypeConfiguration<Veiculo>
{
    public void Configure(EntityTypeBuilder<Veiculo> builder)
    {
        builder.Property(x => x.Foto)
            .HasColumnType("bytea");

        builder.HasOne(x => x.GrupoVeiculo);

        builder.Property(x => x.Marca)
            .IsRequired();

        builder.Property(x => x.Cor)
            .IsRequired();

        builder.Property(x => x.Modelo)
            .IsRequired();

        builder.HasOne(x => x.Combustivel);

        builder.Property(x => x.CapacidadeCombustivel)
            .IsRequired();

        builder.Property(x => x.Ano)
            .IsRequired();
    }
}
