using Locadora.Dominio.ModuloAluguel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Locadora.Infraestrutura.ModuloAluguel;

public class MapeadorDevolucaoEmOrm : IEntityTypeConfiguration<Devolucao>
{
    public void Configure(EntityTypeBuilder<Devolucao> builder)
    {
       builder.Property(x =>x.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(x => x.DataDevolucao)
            .IsRequired();

        builder.Property(x => x.KmDevolucao)
            .IsRequired();

        builder.Property(x => x.LitrosNaChegada)
            .IsRequired();

        builder.Property(x => x.SeguroAcionado) 
            .IsRequired();

        builder.Property(x => x.TanqueCheio)
            .IsRequired();

        builder.Property(x => x.Total)
            .IsRequired();

        builder.HasOne(x => x.Aluguel);
    }
}
