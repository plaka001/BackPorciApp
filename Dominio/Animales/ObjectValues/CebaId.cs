namespace Dominio.Animales.ObjectValues;



public record CebaId(Guid Value)
{
    public static CebaId New() => new(Guid.NewGuid());
}
