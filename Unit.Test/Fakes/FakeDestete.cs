using Dominio.Animales.Lechones;
using Dominio.Animales.ObjectValues;

namespace Unit.Test.Fakes;

public class FakeDestete : Destete
{
    public FakeDestete(
        PartoId partoId = null,
        int destetados = 8,
        int mortalidad = 0,
        decimal pesoPromedio = 6.5m)
        : base(
            DesteteId.New(),
            partoId ?? PartoId.New(),
            DateTime.Now.AddDays(-1),
            destetados,
            mortalidad,
            pesoPromedio,
            "Fake Destete")
    {
    }

    public static FakeDestete CreateDefault() => new();
}
