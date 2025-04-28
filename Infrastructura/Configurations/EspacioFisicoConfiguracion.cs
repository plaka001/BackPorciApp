using Domain.Granjas;
using Dominio.EspacioFisicos;
using Dominio.EspacioFisicos.ObjectValues;
using Dominio.granjas.ObjectValues;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructura.Configurations;

public class EspacioFisicoConfiguration : IEntityTypeConfiguration<EspacioFisico>
{
    public void Configure(EntityTypeBuilder<EspacioFisico> builder)
    {
        builder.ToTable("EspacioFisico");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasConversion(id => id.Value, value => new EspacioFisicoId(value))
            .ValueGeneratedOnAdd();

        builder.Property(e => e.GranjaId)
            .HasConversion(id => id.Value, value => new GranjaId(value))
            .IsRequired();

        builder.Property(e => e.TipoEspacio)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.CantidadEspacios)
            .IsRequired();

        builder.Property(e => e.CapacidadPorEspacio)
            .HasDefaultValue(1)
            .IsRequired();

        builder.Property(e => e.CapacidadRecomendada)
            .IsRequired();

        builder.Property(e => e.FechaCreacion)
            .HasDefaultValueSql("GETDATE()")
            .IsRequired();

        // Configuración como Owned Entity
        builder.OwnsOne(e => e.Capacidad, cb =>
        {
            cb.Property(c => c.CapacidadMaxima)
                .HasColumnName("CapacidadMaxima")
                .IsRequired();

            cb.Property(c => c.CapacidadOcupada)
                .HasColumnName("CapacidadOcupada")
                .IsRequired();
        });

        builder.HasOne<Granja>()
            .WithMany()
            .HasForeignKey(e => e.GranjaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
