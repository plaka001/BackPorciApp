namespace Dominio.Granjas.ObjectValues;

public record ParametrosProduccionId(Guid Value)
{
    public static ParametrosProduccionId New() => new(Guid.NewGuid());
}
