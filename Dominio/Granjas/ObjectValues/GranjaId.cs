namespace Dominio.granjas.ObjectValues;

public record GranjaId(Guid Value)
{
    public static GranjaId New() => new(Guid.NewGuid());
}
