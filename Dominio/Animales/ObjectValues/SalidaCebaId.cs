namespace Dominio.Animales.ObjectValues;


public record SalidaCebaId(Guid Value)
{
    public static SalidaCebaId New() => new(Guid.NewGuid());
}