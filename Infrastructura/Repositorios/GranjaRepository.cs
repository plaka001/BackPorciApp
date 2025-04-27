using Domain.Granjas;
using Dominio.granjas.ObjectValues;
using Dominio.granjas.repository;
using Dominio.Granjas;
using Microsoft.EntityFrameworkCore;

namespace Infrastructura.Repositorios;

internal class GranjaRepository : Repository<Granja, GranjaId>, IGranjaRepository
{
    public GranjaRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public void AgregarParametrosProduccion(ParametrosProduccion entity)
    {
        DbContext.Add(entity);
    }

    public void AgregarParametrosProduccionCalculados(ParametrosCalculados entity)
    {
        DbContext.Add(entity);
    }

    public async Task<ParametrosCalculados?> obtenerParametrsCalculados(GranjaId granjaId)
    {
        return await DbContext.Set<ParametrosCalculados>().FirstOrDefaultAsync(x => x.GranjaId == granjaId)!;
    }

    public async Task<Granja?> ObtenerGranjaByNombre(string Nombre)
    {
        return await DbContext.Set<Granja>().FirstOrDefaultAsync(x => x.Nombre == Nombre)!;
    }
}
