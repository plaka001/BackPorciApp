namespace Dominio.EspacioFisicos.ObjectValues;

public record EspacioFisicoId(Guid Value)
{
    public static EspacioFisicoId New() => new(Guid.NewGuid());
}
