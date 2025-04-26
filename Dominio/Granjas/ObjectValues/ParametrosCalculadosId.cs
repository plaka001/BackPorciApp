namespace Dominio.Granjas.ObjectValues;

public record ParametrosCalculadosId(Guid Value)
{
    public static ParametrosCalculadosId New() => new(Guid.NewGuid());
}