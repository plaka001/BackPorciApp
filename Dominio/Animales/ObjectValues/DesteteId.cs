namespace Dominio.Animales.ObjectValues;


public record DesteteId(Guid Value)
{
    public static DesteteId New() => new(Guid.NewGuid());
}
