using Domain.Granjas;
using Dominio.Granjas;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Dominio.Granjas.ObjectValues;
using Dominio.granjas.ObjectValues;

namespace Infrastructura.Configurations;

internal class ParametrosCalculadosConfiguration : IEntityTypeConfiguration<ParametrosCalculados>
{
    public void Configure(EntityTypeBuilder<ParametrosCalculados> builder)
    {
        builder.ToTable("ParametrosCalculados");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("Id")
            .HasConversion(id => id.Value, value => new ParametrosCalculadosId(value))
            .ValueGeneratedOnAdd();

        builder.Property(p => p.GranjaId)
            .HasColumnName("GranjaId")
            .HasConversion(id => id.Value, value => new GranjaId(value))
            .IsRequired();

        builder.Property(p => p.NumeroGrupos)
            .HasColumnName("NumeroGrupos");

        builder.Property(p => p.CerdasPorGrupo)
            .HasColumnName("CerdasPorGrupo")
            .HasColumnType("decimal(5,2)");

        builder.Property(p => p.EspaciosParideras)
            .HasColumnName("EspaciosParideras");

        builder.Property(p => p.EspaciosMontas)
            .HasColumnName("EspaciosMontas");

        builder.Property(p => p.EspaciosGestacion)
            .HasColumnName("EspaciosGestacion");

        builder.Property(p => p.EspaciosPrecebo)
            .HasColumnName("EspaciosPrecebo");

        builder.Property(p => p.EspaciosCeba)
            .HasColumnName("EspaciosCeba");

        builder.Property(p => p.FechaCalculo)
            .HasColumnName("FechaCalculo")
            .HasDefaultValueSql("GETDATE()");

        // Relación con Granja
        builder.HasOne<Granja>()
            .WithMany()
            .HasForeignKey(p => p.GranjaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}