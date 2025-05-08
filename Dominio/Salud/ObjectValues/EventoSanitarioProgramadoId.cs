namespace Dominio.Salud.ObjectValues;

public record EventoSanitarioProgramadoId(Guid Value)
{
    public static EventoSanitarioProgramadoId New() => new(Guid.NewGuid());
}
