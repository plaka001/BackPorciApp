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
    public Guid? PlanSanitarioId { get; private set; }

    private CerdaCria(
        CerdaCriaId id,
        GranjaId granjaId,
        string identificacion,
        EstadoProductivo estadoProductivo,
        DateTime fechaIngreso,
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
    }

    public static CerdaCria Create(
        GranjaId granjaId,
        string identificacion,
        EstadoProductivo estadoProductivo,
        DateTime fechaIngreso,
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
            espacioFisicoId,
            numeroParto,
            planSanitarioId);
    }

    // Métodos para cambiar estado
    public void CambiarEstado(EstadoProductivo nuevoEstado)
    {
        EstadoProductivo = nuevoEstado;
    }

    public void RegistrarParto(int numeroParto)
    {
        if (numeroParto <= 0)
            throw new ArgumentException("El número de parto debe ser positivo");

        NumeroParto = numeroParto;
        EstadoProductivo = EstadoProductivo.Paridera;
    }
}
