using Dominio.Abstractions;
using Dominio.Salud.ObjectValues;

namespace Dominio.Salud;

public class EventoSanitarioProgramado : Entity<EventoSanitarioProgramadoId>
{
    public int TipoEntidadId { get; private set; } // 1 = Cerda, 2 = LotePrecebo, 3 = LoteCeba
    public Guid EntidadId { get; private set; }
    public PlanSanitarioId PlanSanitarioId { get; private set; }
    public DateTime FechaProgramada { get; private set; }
    public string NombreEvento { get; private set; }
    public string Comentario { get; private set; }
    public bool EstaCompletado { get; private set; }
    public DateTime? FechaCompletado { get; private set; }

    protected EventoSanitarioProgramado(
        EventoSanitarioProgramadoId id,
        int tipoEntidadId,
        Guid entidadId,
        PlanSanitarioId planSanitarioId,
        string nombreEvento,
        DateTime fechaProgramada,
        string comentario) : base(id)
    {
        TipoEntidadId = tipoEntidadId;
        EntidadId = entidadId;
        PlanSanitarioId = planSanitarioId;
        NombreEvento = nombreEvento;
        FechaProgramada = fechaProgramada;
        Comentario = comentario;
        EstaCompletado = false;
    }

    public static EventoSanitarioProgramado Create(
        int tipoEntidadId,
        Guid entidadId,
        PlanSanitarioId planSanitarioId,
        string nombreEvento,
        DateTime fechaProgramada,
        string comentario = null)
    {
        // Validaciones
        if (tipoEntidadId < 1 || tipoEntidadId > 3)
            throw new ArgumentException("Tipo de entidad no válido");

        if (entidadId == Guid.Empty)
            throw new ArgumentException("ID de entidad no válido");

        if (string.IsNullOrWhiteSpace(nombreEvento))
            throw new ArgumentException("El nombre del evento es requerido");

        if (fechaProgramada < DateTime.UtcNow.Date)
            throw new ArgumentException("La fecha programada no puede ser en el pasado");

        return new EventoSanitarioProgramado(
            EventoSanitarioProgramadoId.New(),
            tipoEntidadId,
            entidadId,
            planSanitarioId,
            nombreEvento,
            fechaProgramada,
            comentario);
    }

    public void MarcarComoCompletado()
    {
        EstaCompletado = true;
        FechaCompletado = DateTime.UtcNow;
    }
}

