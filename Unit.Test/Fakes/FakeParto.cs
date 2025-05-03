using Dominio.Animales;
using Dominio.Animales.ObjectValues;

namespace Unit.Test.Fakes;

public class FakeParto : Parto
{
    public FakeParto(
        CerdaCriaId cerdaId = null,
        int vivos = 8,
        int muertos = 2,
        decimal pesoVivos = 1.5m)
        : base(
            PartoId.New(),
            cerdaId ?? CerdaCriaId.New(),
            DateTime.Now.AddDays(-3),
            vivos,
            muertos,
            pesoVivos,
            1.0m,
            false,
            "Fake Parto")
    {
    }

    public static FakeParto CreateForTest() => new();
}