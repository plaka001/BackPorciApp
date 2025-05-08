using Dominio.Salud;
using Dominio.Salud.ObjectValues;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructura.Configurations;

public class EventoSanitarioPlanConfiguration : IEntityTypeConfiguration<EventoSanitarioPlan>
{
    public void Configure(EntityTypeBuilder<EventoSanitarioPlan> builder)
    {
        builder.ToTable("EventoSanitarioPlan");

        // Clave primaria
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("Id")
            .HasConversion(
                id => id.Value,
                value => new EventoSanitarioPlanId(value))
            .ValueGeneratedOnAdd();

        // Configuración EXPLÍCITA de la relación
        builder.HasOne<PlanSanitario>()
            .WithMany(p => p.EventosPlan)  // Referencia explícita a la propiedad de navegación
            .HasForeignKey(e => e.PlanSanitarioId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Configuración de propiedades - Asegúrate que solo hay UNA propiedad para PlanSanitarioId
        builder.Property(e => e.PlanSanitarioId)
            .HasColumnName("PlanSanitarioId")
            .HasConversion(
                id => id.Value,
                value => new PlanSanitarioId(value));

        // Elimina cualquier otra propiedad que pueda estar causando conflicto
        // No debe haber ninguna otra propiedad relacionada con PlanSanitario

        builder.Property(e => e.DiaDesdeAsignacion)
            .HasColumnName("DiaDesdeAsignacion")
            .IsRequired();

        builder.Property(e => e.NombreEvento)
            .HasColumnName("NombreEvento")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.Comentario)
            .HasColumnName("Comentario")
            .HasMaxLength(500);

        // Índices
        builder.HasIndex(e => e.PlanSanitarioId);
    }
}