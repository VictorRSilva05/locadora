using Locadora.Dominio.ModuloCobranca;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Locadora.Infraestrutura.ModuloCobranca;
public class MapeadorCobrancaEmOrm : IEntityTypeConfiguration<Cobranca>
{
    public void Configure(EntityTypeBuilder<Cobranca> builder)
    {
        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.HasOne(x => x.GrupoVeiculo);

        builder.Property(x => x.PlanoCobranca)
            .IsRequired();

        builder.Property(x => x.PrecoDiaria);

        builder.Property(x => x.PrecoKm);

        builder.Property(x => x.KmDisponiveis);

        builder.Property(x => x.PrecoPorKmExtrapolado);

        builder.Property(x => x.Taxa);
    }
}
