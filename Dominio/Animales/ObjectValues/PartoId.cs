namespace Dominio.Animales.ObjectValues;

public record PartoId(Guid Value)
{
    public static PartoId New() => new(Guid.NewGuid());
}
