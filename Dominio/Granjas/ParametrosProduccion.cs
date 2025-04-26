using Dominio.Abstractions;
using Dominio.granjas.ObjectValues;
using Dominio.Granjas.ObjectValues;

namespace Dominio.Granjas;

public class ParametrosProduccion : Entity<ParametrosProduccionId>
{
    public GranjaId GranjaId { get; private set; }

    // Parámetros enteros
    public int DuracionGestacion { get; private set; }
    public int DuracionLactancia { get; private set; }
    public int IntervaloDesteteServicio { get; private set; }
    public int DuracionPrecebo { get; private set; }
    public int DuracionCeba { get; private set; }
    public int ConfirmacionPrenez { get; private set; }
    public int PromedioNacidosVivos { get; private set; }
    public int TiempoAseoDescanso { get; private set; }
    public int RitmoProductivo { get; private set; }
    public int OPP { get; private set; }

    // Parámetros decimales
    public decimal PorcentajeParicion { get; private set; }
    public decimal TasaReemplazo { get; private set; }
    public decimal MortalidadLactancia { get; private set; }
    public decimal MortalidadPrecebo { get; private set; }
    public decimal MortalidadCeba { get; private set; }

    private ParametrosProduccion(
        ParametrosProduccionId id,
        GranjaId granjaId,
        int duracionGestacion,
        int duracionLactancia,
        int intervaloDesteteServicio,
        int duracionPrecebo,
        int duracionCeba,
        decimal porcentajeParicion,
        int confirmacionPrenez,
        decimal tasaReemplazo,
        int promedioNacidosVivos,
        decimal mortalidadLactancia,
        decimal mortalidadPrecebo,
        decimal mortalidadCeba,
        int tiempoAseoDescanso,
        int ritmoProductivo,
        int OPP
    ) : base(id)
    {
        GranjaId = granjaId;
        DuracionGestacion = duracionGestacion;
        DuracionLactancia = duracionLactancia;
        IntervaloDesteteServicio = intervaloDesteteServicio;
        DuracionPrecebo = duracionPrecebo;
        DuracionCeba = duracionCeba;
        PorcentajeParicion = porcentajeParicion;
        ConfirmacionPrenez = confirmacionPrenez;
        TasaReemplazo = tasaReemplazo;
        PromedioNacidosVivos = promedioNacidosVivos;
        MortalidadLactancia = mortalidadLactancia;
        MortalidadPrecebo = mortalidadPrecebo;
        MortalidadCeba = mortalidadCeba;
        TiempoAseoDescanso = tiempoAseoDescanso;
        RitmoProductivo = ritmoProductivo;
        this.OPP = OPP;
    }

    public static ParametrosProduccion Create(
        GranjaId granjaId,
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
        int opp = 7
    )
    {
        return new ParametrosProduccion(
            new ParametrosProduccionId(Guid.NewGuid()),
            granjaId,
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
            opp
        );
    }
}
