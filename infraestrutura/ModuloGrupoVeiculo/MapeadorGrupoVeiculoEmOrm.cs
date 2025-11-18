using Locadora.Dominio.ModuloGrupoVeiculo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Locadora.Infraestrutura.ModuloGrupoVeiculo;
public class MapeadorGrupoVeiculoEmOrm : IEntityTypeConfiguration<GrupoVeiculo>
{
    public void Configure(EntityTypeBuilder<GrupoVeiculo> builder)
    {
        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(x => x.Nome)
            .IsRequired();
    }
}
