using Domain.Granjas;
using Dominio.granjas.ObjectValues;
using Dominio.Granjas;
using Dominio.Granjas.ObjectValues;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructura.Configurations;

internal class ParametrosProduccionConfiguration : IEntityTypeConfiguration<ParametrosProduccion>
{
    public void Configure(EntityTypeBuilder<ParametrosProduccion> builder)
    {
        builder.ToTable("ParametrosProduccion");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("Id")
            .HasConversion(id => id.Value, value => new ParametrosProduccionId(value))
            .ValueGeneratedOnAdd();

        builder.Property(p => p.GranjaId)
            .HasColumnName("GranjaId")
            .HasConversion(id => id.Value, value => new GranjaId(value))
            .IsRequired();

        builder.Property(p => p.DuracionGestacion)
            .HasColumnName("DuracionGestacion")
            .IsRequired();

        builder.Property(p => p.DuracionLactancia)
            .HasColumnName("DuracionLactancia")
            .IsRequired();

        builder.Property(p => p.IntervaloDesteteServicio)
            .HasColumnName("IntervaloDesteteServicio")
            .IsRequired();

        builder.Property(p => p.DuracionPrecebo)
            .HasColumnName("DuracionPrecebo")
            .IsRequired();

        builder.Property(p => p.DuracionCeba)
            .HasColumnName("DuracionCeba")
            .IsRequired();

        builder.Property(p => p.PorcentajeParicion)
            .HasColumnName("PorcentajeParicion")
            .HasColumnType("decimal(5,2)")
            .IsRequired();

        builder.Property(p => p.ConfirmacionPrenez)
            .HasColumnName("ConfirmacionPrenez")
            .IsRequired();

        builder.Property(p => p.TasaReemplazo)
            .HasColumnName("TasaReemplazo")
            .HasColumnType("decimal(5,2)")
            .IsRequired();

        builder.Property(p => p.PromedioNacidosVivos)
            .HasColumnName("PromedioNacidosVivos")
            .IsRequired();

        builder.Property(p => p.MortalidadLactancia)
            .HasColumnName("MortalidadLactancia")
            .HasColumnType("decimal(5,2)");

        builder.Property(p => p.MortalidadPrecebo)
            .HasColumnName("MortalidadPrecebo")
            .HasColumnType("decimal(5,2)");

        builder.Property(p => p.MortalidadCeba)
            .HasColumnName("MortalidadCeba")
            .HasColumnType("decimal(5,2)");

        builder.Property(p => p.TiempoAseoDescanso)
            .HasColumnName("TiempoAseoDescanso");

        builder.Property(p => p.RitmoProductivo)
            .HasColumnName("RitmoProductivo");

        builder.Property(p => p.OPP)
            .HasColumnName("OPP");

        // Relación con Granja
        builder.HasOne<Granja>()
            .WithMany()
            .HasForeignKey(p => p.GranjaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}