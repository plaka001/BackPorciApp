using Dominio.Animales;
using Dominio.Animales.ObjectValues;
using Dominio.Animales.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructura.Repositorios;

internal class AnimalesRepository : Repository<CerdaCria, CerdaCriaId>, IAnimalesRepository
{
    public AnimalesRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public void AgregarCerdaCria(CerdaCria entity)
    {
        DbContext.Add(entity);
    }

    public async Task<CerdaCria?> ObtenerCerdaByIdentificacion(string identificacion)
    {
        return await DbContext.Set<CerdaCria>().FirstOrDefaultAsync(x => x.Identificacion == identificacion)!;
    }
}
