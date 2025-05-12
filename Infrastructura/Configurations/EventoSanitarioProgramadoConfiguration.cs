using Dominio.Salud;
using Dominio.Salud.ObjectValues;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructura.Configurations;

public class EventoSanitarioProgramadoConfiguration : IEntityTypeConfiguration<EventoSanitarioProgramado>
{
    public void Configure(EntityTypeBuilder<EventoSanitarioProgramado> builder)
    {
        builder.ToTable("EventoSanitarioProgramado");

        // Clave primaria
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("Id")
            .HasConversion(
                id => id.Value,
                value => new EventoSanitarioProgramadoId(value))
            .ValueGeneratedOnAdd();

        builder.Property(e => e.TipoEntidadId)
            .HasColumnName("TipoEntidadId")
            .IsRequired();

        builder.Property(e => e.EntidadId)
            .HasColumnName("EntidadId")
            .IsRequired();

        builder.Property(e => e.PlanSanitarioId)
            .HasColumnName("PlanSanitarioId")
            .HasConversion(
                id => id.Value,
                value => new PlanSanitarioId(value));

        builder.Property(e => e.FechaProgramada)
            .HasColumnName("FechaProgramada")
            .HasColumnType("date")
            .IsRequired();

        builder.Property(e => e.NombreEvento)
            .HasColumnName("NombreEvento")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.Comentario)
            .HasColumnName("Comentario")
            .HasMaxLength(500);

        builder.Property(e => e.EstaCompletado)
            .HasColumnName("EstaCompletado")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(e => e.FechaCompletado)
            .HasColumnName("FechaCompletado")
            .HasColumnType("datetime2");

        // Relaciones

        builder.HasOne<PlanSanitario>()
          .WithMany(p => p.Eventos)  // Referencia explícita a la propiedad de navegación
          .HasForeignKey(e => e.PlanSanitarioId)
          .IsRequired()
          .OnDelete(DeleteBehavior.Cascade);


        // Índices
        builder.HasIndex(e => e.TipoEntidadId);
        builder.HasIndex(e => e.EntidadId);
        builder.HasIndex(e => e.PlanSanitarioId);
    }
}
