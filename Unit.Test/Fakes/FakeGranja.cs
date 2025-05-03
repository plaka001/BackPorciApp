using Domain.Granjas;
using Dominio.granjas.ObjectValues;

namespace Unit.Test.Fakes;

public class FakeGranja : Granja
{
    public bool WasCreated { get; private set; }

    public FakeGranja(
        string nombre = "Granja Test",
        string ubicacion = "Ubicación Test",
        int numeroCerdasCria = 100)
        : base(
            id: GranjaId.New(),
            nombre,
            ubicacion,
            numeroCerdasCria,
            fechaCreacion: DateTime.UtcNow)
    {
        WasCreated = true;
    }

    public static FakeGranja CreateDefault() => new();
}
