using Dominio.granjas.ObjectValues;
using Dominio.Salud;
using Dominio.Salud.ObjectValues;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructura.Configurations;


public class PlanSanitarioConfiguration : IEntityTypeConfiguration<PlanSanitario>
{
    public void Configure(EntityTypeBuilder<PlanSanitario> builder)
    {
        builder.ToTable("PlanSanitario");

        // Clave primaria
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("Id")
            .HasConversion(
                id => id.Value,
                value => new PlanSanitarioId(value))
            .ValueGeneratedOnAdd();

        builder.Property(p => p.GranjaId)
            .HasColumnName("GranjaId")
            .HasConversion(
                id => id.Value,
                value => new GranjaId(value))
            .IsRequired();

        builder.Property(p => p.Nombre)
            .HasColumnName("Nombre")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(p => p.TipoAplicacion)
            .HasColumnName("TipoAplicacion")
            .HasConversion<string>()
            .IsRequired();

        builder.Property(p => p.FechaCreacion)
            .HasColumnName("FechaCreacion")
            .HasColumnType("datetime2")
            .HasDefaultValueSql("SYSDATETIME()")
            .IsRequired();

        // Índices
        builder.HasIndex(p => p.GranjaId);
    }
}
