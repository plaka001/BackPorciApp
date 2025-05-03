using Dominio.Animales.Lechones;
using Dominio.Animales.ObjectValues;
using Dominio.EspacioFisicos.ObjectValues;

namespace Unit.Test.Fakes;

public class FakePrecebo : Precebo
{
    // Propiedades para control de tests
    public bool SalidaRegistrada { get; private set; }
    public int CantidadMuertosSimulada { get; set; }

    public FakePrecebo(
        DesteteId desteteId = null,
        int cantidadInicial = 10,
        decimal pesoPromedio = 5.5m,
        EspacioFisicoId espacioId = null,
        string comentario = "Fake Precebo")
        : base(
            PreceboId.New(),
            desteteId ?? DesteteId.New(),
            DateTime.Now.AddDays(-10),
            cantidadInicial,
            pesoPromedio,
            espacioId ?? EspacioFisicoId.New(),
            comentario)
    {
    }

    public override void RegistrarSalida(decimal pesoPromedioFinal, DateTime fechaSalida, int cantidadMuertos)
    {
        SalidaRegistrada = true;
        CantidadMuertosSimulada = cantidadMuertos;
        base.RegistrarSalida(pesoPromedioFinal, fechaSalida, cantidadMuertos);
    }

    // Método helper para tests
    public static FakePrecebo CreateDefault() => new();
}
