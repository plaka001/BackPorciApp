using Domain.Granjas;
using Dominio.granjas.ObjectValues;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructura.Configurations;

internal class GranjaConfiguracion : IEntityTypeConfiguration<Granja>
{
    public void Configure(EntityTypeBuilder<Granja> builder)
    {
        builder.ToTable("Granjas");
        builder.HasKey(g => g.Id);

        builder.Property(g => g.Id)
            .HasColumnName("Id")
            .HasConversion(id => id.Value, value => new GranjaId(value))
            .ValueGeneratedOnAdd();

        builder.Property(g => g.Nombre)
            .HasColumnName("Nombre")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(g => g.Ubicacion)
            .HasColumnName("Ubicacion")
            .HasMaxLength(200);

        builder.Property(g => g.NumeroCerdasCria)
            .HasColumnName("NumeroCerdasCria")
            .IsRequired();

        builder.Property(g => g.FechaCreacion)
            .HasColumnName("FechaCreacion")
            .HasDefaultValueSql("GETDATE()");
    }
}