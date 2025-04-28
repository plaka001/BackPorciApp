using Dominio.Animales;
using Dominio.Animales.ObjectValues;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructura.Configurations;

public class PartoConfiguration : IEntityTypeConfiguration<Parto>
{
    public void Configure(EntityTypeBuilder<Parto> builder)
    {
        builder.ToTable("Parto");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(id => id.Value, value => new PartoId(value));

        builder.Property(p => p.CerdaCriaId)
            .HasConversion(id => id.Value, value => new CerdaCriaId(value));

        builder.Property(p => p.FechaParto)
            .IsRequired();

        builder.Property(p => p.PesoPromedioVivos)
            .HasColumnType("decimal(5,2)");

        builder.Property(p => p.PesoPromedioMuertos)
            .HasColumnType("decimal(5,2)");
    }
}