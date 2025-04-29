using Dominio.Abstractions;
using Dominio.Animales.ObjectValues;
using Dominio.EspacioFisicos.ObjectValues;
using Dominio.granjas.ObjectValues;

namespace Dominio.Animales;

public class CerdaCria : Entity<CerdaCriaId>
{
    public GranjaId GranjaId { get; private set; }
    public string Identificacion { get; private set; }
    public EstadoProductivo EstadoProductivo { get; private set; }
    public DateTime FechaIngreso { get; private set; }
    public EspacioFisicoId EspacioFisicoId { get; private set; }
    public int? NumeroParto { get; private set; }
    public DateTime FechaCreacion { get; private set; }
    public DateTime FechaUltimoTraslado { get; private set; }
    public Guid? PlanSanitarioId { get; private set; }

    private CerdaCria(
        CerdaCriaId id,
        GranjaId granjaId,
        string identificacion,
        EstadoProductivo estadoProductivo,
        DateTime fechaIngreso,
        DateTime fechaUltimoTraslado,
        EspacioFisicoId espacioFisicoId,
        int? numeroParto,
        Guid? planSanitarioId) : base(id)
    {
        GranjaId = granjaId;
        Identificacion = identificacion;
        EstadoProductivo = estadoProductivo;
        FechaIngreso = fechaIngreso;
        EspacioFisicoId = espacioFisicoId;
        NumeroParto = numeroParto;
        PlanSanitarioId = planSanitarioId;
        FechaCreacion = DateTime.UtcNow;
        FechaUltimoTraslado = fechaUltimoTraslado;
    }

    public static CerdaCria Create(
        GranjaId granjaId,
        string identificacion,
        EstadoProductivo estadoProductivo,
        DateTime fechaIngreso,
        DateTime fechaUltimoTraslado,
        EspacioFisicoId espacioFisicoId,
        int? numeroParto = null,
        Guid? planSanitarioId = null)
    {
        // Validaciones
        if (string.IsNullOrWhiteSpace(identificacion))
            throw new ArgumentException("La identificación no puede estar vacía");

        if (fechaIngreso > DateTime.UtcNow)
            throw new ArgumentException("La fecha de ingreso no puede ser futura");

        return new CerdaCria(
            CerdaCriaId.New(),
            granjaId,
            identificacion,
            estadoProductivo,
            fechaIngreso,
            fechaUltimoTraslado,
            espacioFisicoId,
            numeroParto,
            planSanitarioId);
    }

    // Métodos para cambiar estado
    public void CambiarEstado(EstadoProductivo nuevoEstado)
    {
        EstadoProductivo = nuevoEstado;
    }

    public void RegistrarParto()
    {
        NumeroParto += 1;
        EstadoProductivo = EstadoProductivo.Paridera;
    }

    public void Trasladar(EspacioFisicoId nuevoEspacioId, DateTime fechaTraslado, EstadoProductivo estadoProductivo)
    {
        if (nuevoEspacioId == null)
            throw new ArgumentException("El espacio físico no puede ser nulo");

        if (fechaTraslado > DateTime.UtcNow)
            throw new ArgumentException("La fecha de traslado no puede ser futura");

        EspacioFisicoId = nuevoEspacioId;
        FechaUltimoTraslado = fechaTraslado;
        EstadoProductivo = estadoProductivo;

        // Opcional: Evento de dominio para notificar el traslado
        // AddDomainEvent(new CerdaTrasladada(Id, nuevoEspacioId, fechaTraslado));
    }
}
