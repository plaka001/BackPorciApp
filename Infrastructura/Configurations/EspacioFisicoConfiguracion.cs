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

        // Configuración de la clave primaria
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("Id")
            .HasConversion(
                id => id.Value,
                value => new EspacioFisicoId(value))
            .ValueGeneratedOnAdd();

        // Configuración para GranjaId (clave foránea)
        builder.Property(e => e.GranjaId)
            .HasColumnName("GranjaId")
            .HasConversion(
                id => id.Value,
                value => new GranjaId(value))
            .IsRequired();

        // Configuración para TipoEspacio
        builder.Property(e => e.TipoEspacio)
            .HasColumnName("TipoEspacio")
            .HasMaxLength(50)
            .IsRequired();

        // Configuración para CantidadEspacios
        builder.Property(e => e.CantidadEspacios)
            .HasColumnName("CantidadEspacios")
            .IsRequired();

        // Configuración para CapacidadPorEspacio con valor por defecto
        builder.Property(e => e.CapacidadPorEspacio)
            .HasColumnName("CapacidadPorEspacio")
            .HasDefaultValue(1)
            .IsRequired();

        // Configuración para CapacidadRecomendada
        builder.Property(e => e.CapacidadRecomendada)
            .HasColumnName("CapacidadRecomendada")
            .IsRequired();

        // Configuración para CapacidadReal (calculada)
        builder.Property(e => e.CapacidadReal)
            .HasColumnName("CapacidadReal")
            .IsRequired();

        // Configuración para FechaCreación con valor por defecto
        builder.Property(e => e.FechaCreacion)
            .HasColumnName("FechaCreacion")
            .HasDefaultValueSql("GETDATE()")
            .IsRequired();

        // Configuración de la relación con Granja
        builder.HasOne<Granja>()
            .WithMany()
            .HasForeignKey(e => e.GranjaId)
            .OnDelete(DeleteBehavior.Restrict);

        // Índices para mejorar el rendimiento
        builder.HasIndex(e => e.GranjaId);
        builder.HasIndex(e => e.TipoEspacio);
    }
}
