namespace Dominio.Salud.ObjectValues;

public record PlanSanitarioId(Guid Value)
{
    public static PlanSanitarioId New() => new(Guid.NewGuid());
}
