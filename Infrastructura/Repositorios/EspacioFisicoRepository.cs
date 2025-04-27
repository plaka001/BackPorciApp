
using Dominio.EspacioFisicos.ObjectValues;
using Dominio.EspacioFisicos.Repository;
using Dominio.EspacioFisicos;

namespace Infrastructura.Repositorios;

internal class EspacioFisicoRepository : Repository<EspacioFisico, EspacioFisicoId>, IEspacioFisicoRepository
{
    public EspacioFisicoRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

}
