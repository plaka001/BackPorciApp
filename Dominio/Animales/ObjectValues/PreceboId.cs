namespace Dominio.Animales.ObjectValues;

public record PreceboId(Guid Value)
{
    public static PreceboId New() => new(Guid.NewGuid());
}