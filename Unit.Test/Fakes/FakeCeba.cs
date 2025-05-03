using Dominio.Animales.Lechones;
using Dominio.Animales.ObjectValues;
using Dominio.EspacioFisicos.ObjectValues;

namespace Unit.Test.Fakes;

public class FakeCeba : Ceba
{
    public bool SalidaRegistrada { get; private set; }

    public FakeCeba(
        PreceboId preceboId = null,
        int cantidadInicial = 15,
        decimal pesoPromedio = 20.0m,
        EspacioFisicoId espacioId = null)
        : base(
            CebaId.New(),
            preceboId ?? PreceboId.New(),
            DateTime.Now.AddDays(-5),
            cantidadInicial,
            pesoPromedio,
            espacioId ?? EspacioFisicoId.New(),
            "Fake Ceba")
    {
    }

    public override void RegistrarSalida(decimal pesoPromedioFinal, DateTime fechaSalida, int cantidadMuertos)
    {
        SalidaRegistrada = true;
        base.RegistrarSalida(pesoPromedioFinal, fechaSalida, cantidadMuertos);
    }

    public static FakeCeba CreateWithDefaults() => new();
}
