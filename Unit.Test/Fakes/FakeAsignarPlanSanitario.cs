using Aplicacion.Salud.PlanesSanitarios.Asignar;
using Dominio.granjas.ObjectValues;
using Dominio.Salud;
using Dominio.Salud.Enums;

namespace Unit.Test.Fakes;

public class FakeAsignarPlanSanitario
{
    public static AsignarPlanSanitarioCommand Default() =>
       new AsignarPlanSanitarioCommand(
           PlanSanitarioId: Guid.NewGuid(),
           TipoEntidadId: (int)TipoAplicacionSanitaria.Cerda,
           EntidadId: Guid.NewGuid(),
           //menos 3 horas
           FechaAsignacion : DateTime.Now.AddHours(3)
       );

    public static PlanSanitario PlanConEventos()
    {
        var plan = PlanSanitario.Create(
            new GranjaId(Guid.NewGuid()),
            "Plan Test",
            TipoAplicacionSanitaria.Cerda
        );

        plan.AgregarEventoPlan(0, "Vacuna 1", "Comentario 1");
        plan.AgregarEventoPlan(3, "Vacuna 2", "Comentario 2");

        return plan;
    }
}
