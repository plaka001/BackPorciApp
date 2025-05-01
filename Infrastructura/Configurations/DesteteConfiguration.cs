using Dominio.Animales;
using Dominio.Animales.Lechones;
using Dominio.Animales.ObjectValues;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructura.Configurations
{
    public class DesteteConfiguration : IEntityTypeConfiguration<Destete>
    {
        public void Configure(EntityTypeBuilder<Destete> builder)
        {
            builder.ToTable("Destete");

            // Clave primaria
            builder.HasKey(d => d.Id);

            builder.Property(d => d.Id)
                .HasColumnName("Id")
                .HasConversion(
                    id => id.Value,
                    value => new DesteteId(value))
                .ValueGeneratedOnAdd();

            // Relación con Parto
            builder.Property(d => d.PartoId)
                .HasColumnName("PartoId")
                .HasConversion(
                    id => id.Value,
                    value => new PartoId(value))
                .IsRequired();

            builder.Property(d => d.FechaDestete)
                .HasColumnName("FechaDestete")
                .HasColumnType("datetime2")
                .IsRequired();

            builder.Property(d => d.CantidadDestetados)
                .HasColumnName("CantidadDestetados")
                .IsRequired();

            builder.Property(d => d.MortalidadPreDestete)
                .HasColumnName("MortalidadPreDestete")
                .IsRequired();

            builder.Property(d => d.PesoPromedioDestetados)
                .HasColumnName("PesoPromedioDestetados")
                .HasColumnType("decimal(5,2)")
                .IsRequired();

            builder.Property(d => d.Comentario)
                .HasColumnName("Comentario")
                .HasMaxLength(500);

            builder.Property(d => d.FechaCreacion)
                .HasColumnName("FechaCreacion")
                .HasColumnType("datetime2")
                .HasDefaultValueSql("SYSDATETIME()")
                .IsRequired();

            // Configuración de relaciones
            builder.HasOne<Parto>()
                .WithMany()
                .HasForeignKey(d => d.PartoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índices
            builder.HasIndex(d => d.PartoId);
            builder.HasIndex(d => d.FechaDestete);
        }
    }
}