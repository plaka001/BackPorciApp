using Aplicacion.Animales.CerdaDeCria.Prenez;

namespace Unit.Test.Fakes;

public static class FakeConfirmarPrenez
{
    public static ConfirmarPrenezCerdaCommand Default(bool estaPrenada = true) =>
        new ConfirmarPrenezCerdaCommand(
            IdentificacionCerda: "CERDA123",
            FechaConfirmacion: DateTime.Now.AddHours(-1),
            EstaPreñada: estaPrenada
        );
}