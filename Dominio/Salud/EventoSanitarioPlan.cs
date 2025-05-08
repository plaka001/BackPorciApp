using Dominio.Abstractions;
using Dominio.Salud.ObjectValues;

namespace Dominio.Salud;

public class EventoSanitarioPlan : Entity<EventoSanitarioPlanId>
{
    public PlanSanitarioId PlanSanitarioId { get; private set; }
    public int DiaDesdeAsignacion { get; private set; }
    public string NombreEvento { get; private set; }
    public string Comentario { get; private set; }

    protected EventoSanitarioPlan(
        EventoSanitarioPlanId id,
        PlanSanitarioId planSanitarioId,
        int diaDesdeAsignacion,
        string nombreEvento,
        string comentario) : base(id)
    {
        PlanSanitarioId = planSanitarioId;
        DiaDesdeAsignacion = diaDesdeAsignacion;
        NombreEvento = nombreEvento;
        Comentario = comentario;
    }

    public static EventoSanitarioPlan Create(
        PlanSanitarioId planSanitarioId,
        int diaDesdeAsignacion,
        string nombreEvento,
        string comentario = null)
    {
        // Validaciones
        if (planSanitarioId == null)
            throw new ArgumentException("El ID del plan sanitario es requerido");

        if (diaDesdeAsignacion < 0)
            throw new ArgumentException("El día desde asignación no puede ser negativo");

        if (string.IsNullOrWhiteSpace(nombreEvento))
            throw new ArgumentException("El nombre del evento es requerido");

        return new EventoSanitarioPlan(
            EventoSanitarioPlanId.New(),
            planSanitarioId,
            diaDesdeAsignacion,
            nombreEvento,
            comentario);
    }

    public void ActualizarInformacion(
        int diaDesdeAsignacion,
        string nombreEvento,
        string comentario)
    {
        if (diaDesdeAsignacion < 0)
            throw new ArgumentException("El día desde asignación no puede ser negativo");

        if (string.IsNullOrWhiteSpace(nombreEvento))
            throw new ArgumentException("El nombre del evento es requerido");

        DiaDesdeAsignacion = diaDesdeAsignacion;
        NombreEvento = nombreEvento;
        Comentario = comentario;
    }
}
