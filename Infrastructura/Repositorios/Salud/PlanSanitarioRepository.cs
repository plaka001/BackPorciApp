using Dominio.Salud;
using Dominio.Salud.ObjectValues;
using Dominio.Salud.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructura.Repositorios.Salud;


internal class PlanSanitarioRepository : Repository<PlanSanitario, PlanSanitarioId>, IPlanSanitarioRepository
{
    public PlanSanitarioRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public void Agregar(EventoSanitarioPlan entity)
    {
        DbContext.Add(entity);
    }

    public async Task<List<EventoSanitarioPlan>?> ObtenerSegunIdPlan(PlanSanitarioId id, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<EventoSanitarioPlan>().Where(x=> x.PlanSanitarioId == id).ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<PlanSanitario?> ObtenerSegunNombre(string nombre, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<PlanSanitario>().FirstOrDefaultAsync(x => x.Nombre == nombre)!;
    }

    public async Task<PlanSanitario?> ObtenerPorIdConEventosAsync(PlanSanitarioId id, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<PlanSanitario>()
            .Include(p => p.EventosPlan)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public void Agregar(IEnumerable<EventoSanitarioProgramado> eventos)
    {
        DbContext.AddRange(eventos);
    }
}