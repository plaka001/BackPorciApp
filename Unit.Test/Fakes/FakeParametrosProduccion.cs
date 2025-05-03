using Dominio.granjas.ObjectValues;
using Dominio.Granjas.ObjectValues;
using Dominio.Granjas;

namespace Unit.Test.Fakes;

public class FakeParametrosProduccion : ParametrosProduccion
{
    public bool WasCalculated { get; private set; }

    public FakeParametrosProduccion(
        GranjaId granjaId = null,
        int duracionGestacion = 114,
        int duracionLactancia = 21,
        int intervaloDesteteServicio = 5,
        int duracionPrecebo = 49,
        int duracionCeba = 91,
        decimal porcentajeParicion = 88m,
        int confirmacionPrenez = 30,
        decimal tasaReemplazo = 40m,
        int promedioNacidosVivos = 12,
        decimal mortalidadLactancia = 5m,
        decimal mortalidadPrecebo = 2m,
        decimal mortalidadCeba = 1m,
        int tiempoAseoDescanso = 7,
        int ritmoProductivo = 30,
        int opp = 7)
        : base(
            new ParametrosProduccionId(Guid.NewGuid()),
            granjaId ?? GranjaId.New(),
            duracionGestacion,
            duracionLactancia,
            intervaloDesteteServicio,
            duracionPrecebo,
            duracionCeba,
            porcentajeParicion,
            confirmacionPrenez,
            tasaReemplazo,
            promedioNacidosVivos,
            mortalidadLactancia,
            mortalidadPrecebo,
            mortalidadCeba,
            tiempoAseoDescanso,
            ritmoProductivo,
            opp)
    {
    }

    public static FakeParametrosProduccion CreateWithDefaults() => new();
}
