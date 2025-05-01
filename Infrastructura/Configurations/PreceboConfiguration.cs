using Dominio.Animales.Lechones;
using Dominio.Animales.ObjectValues;
using Dominio.EspacioFisicos;
using Dominio.EspacioFisicos.ObjectValues;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructura.Configurations
{
    public class PreceboConfiguration : IEntityTypeConfiguration<Precebo>
    {
        public void Configure(EntityTypeBuilder<Precebo> builder)
        {
            builder.ToTable("Precebo");

            // Clave primaria
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .HasColumnName("Id")
                .HasConversion(
                    id => id.Value,
                    value => new PreceboId(value))
                .ValueGeneratedOnAdd();

            // Relación con Destete
            builder.Property(p => p.DesteteId)
                .HasColumnName("DesteteId")
                .HasConversion(
                    id => id.Value,
                    value => new DesteteId(value))
                .IsRequired();

            builder.Property(p => p.FechaIngreso)
                .HasColumnName("FechaIngreso")
                .HasColumnType("datetime2")
                .IsRequired();

            builder.Property(p => p.CantidadInicial)
                .HasColumnName("CantidadInicial")
                .IsRequired();

            builder.Property(p => p.PesoPromedioInicial)
                .HasColumnName("PesoPromedioInicial")
                .HasColumnType("decimal(5,2)")
                .IsRequired();

            // Relación con EspacioFisico
            builder.Property(p => p.EspacioFisicoId)
                .HasColumnName("EspacioFisicoId")
                .HasConversion(
                    id => id.Value,
                    value => new EspacioFisicoId(value))
                .IsRequired();

            builder.Property(p => p.Comentario)
                .HasColumnName("Comentario")
                .HasMaxLength(500);

            builder.Property(p => p.FechaCreacion)
                .HasColumnName("FechaCreacion")
                .HasColumnType("datetime2")
                .HasDefaultValueSql("SYSDATETIME()")
                .IsRequired();

            builder.Property(p => p.CantidadMuertos)
                .HasColumnName("CantidadMuertos")
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(p => p.PesoPromedioFinal)
                .HasColumnName("PesoPromedioFinal")
                .HasColumnType("decimal(5,2)");

            builder.Property(p => p.FechaSalida)
                .HasColumnName("FechaSalida")
                .HasColumnType("datetime2");

            // Configuración de relaciones
            builder.HasOne<Destete>()
                .WithMany()
                .HasForeignKey(p => p.DesteteId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<EspacioFisico>()
                .WithMany()
                .HasForeignKey(p => p.EspacioFisicoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índices
            builder.HasIndex(p => p.DesteteId);
            builder.HasIndex(p => p.EspacioFisicoId);
            builder.HasIndex(p => p.FechaIngreso);
            builder.HasIndex(p => p.FechaSalida);
        }
    }
}