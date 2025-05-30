using Dominio.Animales.CerdasCria;
using Dominio.Animales.General;
using Dominio.Animales.ObjectValues;
using Dominio.EspacioFisicos.ObjectValues;
using Dominio.granjas.ObjectValues;

namespace Unit.Test.Fakes;


public class FakeCerdaCria : CerdaCria
{
    // Propiedades adicionales para controlar comportamiento en tests
    public bool RegistrarPartoLlamado { get; private set; }
    public bool TrasladarLlamado { get; private set; }

    public FakeCerdaCria(
        GranjaId granjaId = null,
        string identificacion = "ID-123",
        EstadoProductivo estadoProductivo = EstadoProductivo.Gestante,
        EspacioFisicoId espacioFisicoId = null,
        int? numeroParto = null,
        Guid? planSanitarioId = null)
        : base(
            id: CerdaCriaId.New(),
            granjaId ?? GranjaId.New(),
            identificacion,
            estadoProductivo,
            fechaIngreso: DateTime.Now.AddDays(-10),
            fechaUltimoTraslado: DateTime.Now.AddDays(-5),
            espacioFisicoId ?? EspacioFisicoId.New(),
            numeroParto,
            planSanitarioId)
    {
    }

    // Sobrescribimos métodos para rastrear llamadas
    public override void RegistrarParto()
    {
        RegistrarPartoLlamado = true;
        base.RegistrarParto(); // Opcional: Ejecuta la lógica real si es necesario
    }

    public new void Trasladar(EspacioFisicoId nuevoEspacioId, DateTime fechaTraslado, EstadoProductivo estadoProductivo)
    {
        TrasladarLlamado = true;
        base.Trasladar(nuevoEspacioId, fechaTraslado, estadoProductivo);
    }

    // Método de fábrica para simplificar creación
    public static FakeCerdaCria CreateDefault(EstadoProductivo estadoProductivo)
    {
        return new(numeroParto: 1,estadoProductivo: estadoProductivo);
    }

    internal static FakeCerdaCria ConEstado(EstadoProductivo estadoProductivo)
    {
        return new( estadoProductivo: estadoProductivo);
    }
}

