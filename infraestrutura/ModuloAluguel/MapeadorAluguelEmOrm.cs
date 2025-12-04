using Locadora.Dominio.ModuloAluguel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Locadora.Infraestrutura.ModuloAluguel;
public class MapeadorAluguelEmOrm : IEntityTypeConfiguration<Aluguel>
{
    public void Configure(EntityTypeBuilder<Aluguel> builder)
    {
        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.HasOne(x => x.Condutor);

        builder.HasOne(x => x.Cliente);

        builder.HasOne(x => x.Funcionario);

        builder.HasOne(x => x.Cobranca);

        builder.Property(x => x.Caucao)
            .IsRequired();

        builder.HasOne(x => x.Veiculo);

        builder.Property(x => x.DataSaida)
            .IsRequired();

        builder.Property(x => x.DataRetornoPrevista)
            .IsRequired();

        builder.HasMany(x => x.Taxas);

        builder.Property(x => x.KmInicial)
            .IsRequired();

        builder.Property(x => x.Status)
            .IsRequired();
    }
}
