using Domain.Granjas;
using Dominio.Animales;
using Dominio.Animales.ObjectValues;
using Dominio.EspacioFisicos;
using Dominio.EspacioFisicos.ObjectValues;
using Dominio.granjas.ObjectValues;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructura.Configurations;

public class CerdaCriaConfiguration : IEntityTypeConfiguration<CerdaCria>
{
    public void Configure(EntityTypeBuilder<CerdaCria> builder)
    {
        builder.ToTable("CerdaCria");

        // Clave primaria
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("Id")
            .HasConversion(
                id => id.Value,
                value => new CerdaCriaId(value))
            .ValueGeneratedOnAdd();

        // Relación con Granja
        builder.Property(c => c.GranjaId)
            .HasColumnName("GranjaId")
            .HasConversion(
                id => id.Value,
                value => new GranjaId(value))
            .IsRequired();

        builder.Property(c => c.Identificacion)
            .HasColumnName("Identificacion")
            .HasMaxLength(100)
            .IsRequired();

        // Configuración para el enum EstadoProductivo
        builder.Property(c => c.EstadoProductivo)
            .HasColumnName("EstadoProductivo")
            .HasConversion(
                v => (int)v,
                v => (EstadoProductivo)v)
            .IsRequired();

        builder.Property(c => c.FechaIngreso)
            .HasColumnName("FechaIngreso")
            .HasColumnType("datetime2")
            .IsRequired();

        // Relación con EspacioFisico
        builder.Property(c => c.EspacioFisicoId)
            .HasColumnName("EspacioFisicoId")
            .HasConversion(
                id => id.Value,
                value => new EspacioFisicoId(value))
            .IsRequired();

        builder.Property(c => c.NumeroParto)
            .HasColumnName("NumeroParto");

        builder.Property(c => c.FechaCreacion)
            .HasColumnName("FechaCreacion")
            .HasColumnType("datetime2")
            .HasDefaultValueSql("SYSDATETIME()")
            .IsRequired();

        builder.Property(c => c.PlanSanitarioId)
            .HasColumnName("PlanSanitarioId");

        // Configuración de relaciones
        builder.HasOne<Granja>()
            .WithMany()
            .HasForeignKey(c => c.GranjaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<EspacioFisico>()
            .WithMany()
            .HasForeignKey(c => c.EspacioFisicoId)
            .OnDelete(DeleteBehavior.Restrict);

        // Índices para mejorar el rendimiento
        builder.HasIndex(c => c.GranjaId);
        builder.HasIndex(c => c.EspacioFisicoId);
        builder.HasIndex(c => c.EstadoProductivo);
        builder.HasIndex(c => c.Identificacion).IsUnique();
    }
}
