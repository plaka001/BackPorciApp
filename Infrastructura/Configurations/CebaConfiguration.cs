using Dominio.Animales;
using Dominio.Animales.Lechones;
using Dominio.Animales.ObjectValues;
using Dominio.EspacioFisicos;
using Dominio.EspacioFisicos.ObjectValues;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructura.Configurations
{
    public class CebaConfiguration : IEntityTypeConfiguration<Ceba>
    {
        public void Configure(EntityTypeBuilder<Ceba> builder)
        {
            builder.ToTable("Ceba");

            // Clave primaria
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("Id")
                .HasConversion(
                    id => id.Value,
                    value => new CebaId(value))
                .ValueGeneratedOnAdd();

            // Relación con Precebo
            builder.Property(c => c.PreceboId)
                .HasColumnName("PreceboId")
                .HasConversion(
                    id => id.Value,
                    value => new PreceboId(value))
                .IsRequired();

            builder.Property(c => c.FechaIngreso)
                .HasColumnName("FechaIngreso")
                .HasColumnType("datetime2")
                .IsRequired();

            builder.Property(c => c.CantidadInicial)
                .HasColumnName("CantidadInicial")
                .IsRequired();

            builder.Property(c => c.CantidadMuertos)
                .HasColumnName("CantidadMuertos")
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(c => c.PesoPromedioInicial)
                .HasColumnName("PesoPromedioInicial")
                .HasColumnType("decimal(5,2)")
                .IsRequired();

            builder.Property(c => c.PesoPromedioFinal)
                .HasColumnName("PesoPromedioFinal")
                .HasColumnType("decimal(5,2)");

            builder.Property(c => c.FechaSalida)
                .HasColumnName("FechaSalida")
                .HasColumnType("datetime2");

            // Relación con EspacioFisico
            builder.Property(c => c.EspacioFisicoId)
                .HasColumnName("EspacioFisicoId")
                .HasConversion(
                    id => id.Value,
                    value => new EspacioFisicoId(value))
                .IsRequired();

            builder.Property(c => c.Comentario)
                .HasColumnName("Comentario")
                .HasMaxLength(500);

            builder.Property(c => c.FechaCreacion)
                .HasColumnName("FechaCreacion")
                .HasColumnType("datetime2")
                .HasDefaultValueSql("SYSDATETIME()")
                .IsRequired();

            // Configuración de relaciones
            builder.HasOne<Precebo>()
                .WithMany()
                .HasForeignKey(c => c.PreceboId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<EspacioFisico>()
                .WithMany()
                .HasForeignKey(c => c.EspacioFisicoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índices
            builder.HasIndex(c => c.PreceboId);
            builder.HasIndex(c => c.EspacioFisicoId);
        }
    }
}