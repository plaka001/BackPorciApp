using Aplicacion.Salud.PlanesSanitarios.Crear;
using Dominio.granjas.ObjectValues;
using Dominio.Salud;
using Dominio.Salud.Enums;
using Dominio.Salud.ObjectValues;
using Aplicacion.Dtos;

namespace Unit.Test.Fakes;

public class FakePlanSanitario : PlanSanitario
{
    public FakePlanSanitario()
        : base(PlanSanitarioId.New(), new GranjaId(Guid.NewGuid()), "Plan Existente", TipoAplicacionSanitaria.Cerda) { }
}


public static class FakeCrearPlanSanitarioCommand
{
    public static CrearPlanSanitarioCommand Default() =>
        new CrearPlanSanitarioCommand(
            GranjaId: Guid.NewGuid(),
            Nombre: "Plan Vacunación 1",
            TipoAplicacion: TipoAplicacionSanitaria.Cerda,
            Eventos: new List<EventoSanitarioDto>
            {
                new EventoSanitarioDto(0, "Vacuna A", "Primera dosis"),
                new EventoSanitarioDto(7, "Refuerzo", "Segunda dosis")
            }
        );

    public static CrearPlanSanitarioCommand WithoutEventos() =>
        new CrearPlanSanitarioCommand(
            GranjaId: Guid.NewGuid(),
            Nombre: "Plan Sin Eventos",
            TipoAplicacion: TipoAplicacionSanitaria.Cerda,
            Eventos: new List<EventoSanitarioDto>() // lista vacía
        );
}