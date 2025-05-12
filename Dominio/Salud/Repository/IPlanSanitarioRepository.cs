using Dominio.Salud.ObjectValues;

namespace Dominio.Salud.Repository;

public interface IPlanSanitarioRepository
{

    public void Actualizar(PlanSanitario entity);
    Task<PlanSanitario?> ObtenerSegunNombre(string nonbre, CancellationToken cancellationToken);

    public void Agregar(PlanSanitario entity);

    public void Agregar(IEnumerable<EventoSanitarioProgramado> eventos);

    public void Agregar(EventoSanitarioPlan entity);

    Task<PlanSanitario?> ObtenerSegunId(
        PlanSanitarioId id,
        CancellationToken cancellationToken = default
    );

    Task<List<EventoSanitarioPlan>?> ObtenerSegunIdPlan(
      PlanSanitarioId id,
      CancellationToken cancellationToken = default
  );

    Task<PlanSanitario?> ObtenerPorIdConEventosAsync(PlanSanitarioId id, CancellationToken cancellationToken = default);
}
