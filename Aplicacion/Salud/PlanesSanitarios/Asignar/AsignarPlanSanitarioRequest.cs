namespace Aplicacion.Salud.PlanesSanitarios.Asignar;

public record AsignarPlanSanitarioRequest(
      Guid PlanSanitarioId,
  int TipoEntidadId,
  Guid EntidadId,
  DateTime FechaAsignacion
);
