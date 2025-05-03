using Dominio.Animales;
using Dominio.Animales.Lechones;
using Dominio.Animales.ObjectValues;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructura.Configurations
{
    public class SalidaCebaConfiguration : IEntityTypeConfiguration<SalidaCeba>
    {
        public void Configure(EntityTypeBuilder<SalidaCeba> builder)
        {
            builder.ToTable("SalidaCeba");

            // Clave primaria
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .HasColumnName("Id")
                .HasConversion(
                    id => id.Value,
                    value => new SalidaCebaId(value))
                .ValueGeneratedOnAdd();

            // Relación con Ceba
            builder.Property(s => s.CebaId)
                .HasColumnName("CebaId")
                .HasConversion(
                    id => id.Value,
                    value => new CebaId(value))
                .IsRequired();

            builder.Property(s => s.FechaSalida)
                .HasColumnName("FechaSalida")
                .HasColumnType("datetime2")
                .IsRequired();

            builder.Property(s => s.PesoPromedioFinal)
                .HasColumnName("PesoPromedioFinal")
                .HasColumnType("decimal(5,2)")
                .IsRequired();

            builder.Property(s => s.CantidadMuertos)
                .HasColumnName("CantidadMuertos")
                .IsRequired();

            builder.Property(s => s.CantidadVivos)
                .HasColumnName("CantidadVivos")
                .IsRequired();

            builder.Property(s => s.FechaCreacion)
                .HasColumnName("FechaCreacion")
                .HasColumnType("datetime2")
                .HasDefaultValueSql("SYSDATETIME()")
                .IsRequired();

            // Configuración de relaciones
            builder.HasOne<Ceba>()
                .WithMany()
                .HasForeignKey(s => s.CebaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índices
            builder.HasIndex(s => s.CebaId);
            builder.HasIndex(s => s.FechaSalida);
        }
    }
}