namespace Dominio.Salud.ObjectValues;


public record EventoSanitarioPlanId(Guid Value)
{
    public static EventoSanitarioPlanId New() => new(Guid.NewGuid());
}
