using Dominio.Animales.CerdasCria;
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

    public async Task<PlanSanitario?> ObtenerSegunNombre(string nombre, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<PlanSanitario>().FirstOrDefaultAsync(x => x.Nombre == nombre)!;
    }
}