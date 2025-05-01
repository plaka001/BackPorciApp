
using Dominio.EspacioFisicos.ObjectValues;
using Dominio.EspacioFisicos.Repository;
using Dominio.EspacioFisicos;
using Dominio.granjas.ObjectValues;
using Microsoft.EntityFrameworkCore;

namespace Infrastructura.Repositorios;

internal class EspacioFisicoRepository : Repository<EspacioFisico, EspacioFisicoId>, IEspacioFisicoRepository
{
    public EspacioFisicoRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<EspacioFisico?> ObtenerSegunTipo(TipoEspacio tipoEspacio ,CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<EspacioFisico>()
            .Where(e => e.TipoEspacio == tipoEspacio.ToString() &&
                        e.Capacidad.CapacidadOcupada < e.Capacidad.CapacidadMaxima)
            .OrderBy(e => e.Capacidad.CapacidadOcupada) // opcional: para usar el menos lleno
            .FirstOrDefaultAsync(cancellationToken);
    }

}
