using Aplicacion.Abstractions.Messaging;

namespace Aplicacion.Salud.PlanesSanitarios.Asignar;

public record class AsignarPlanSanitarioCommand(
    Guid PlanSanitarioId,
    int TipoEntidadId,
    Guid EntidadId,
    DateTime FechaAsignacion
) : ICommand;
