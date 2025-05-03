using Dominio.Abstractions;
using Dominio.Animales;
using Dominio.Animales.CerdasCria;
using Dominio.Animales.Lechones;
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


    public void AgregarParto(Parto entity)
    {
        DbContext.Add(entity);
    }

    public void AgregarDestete(Destete entity)
    {
        DbContext.Add(entity);
    }

    public async Task<Parto?> ObtenerSegunId(PartoId id, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<Parto>().FirstOrDefaultAsync(x => x.Id == id) ;
    }

    public async Task<Destete?> ObtenerSegunId(DesteteId id, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<Destete>().FirstOrDefaultAsync(x => x.Id == id)!;
    }

    public void agregarPrecebo(Precebo entity)
    {
        DbContext.Add(entity);
    }

    public async Task<Precebo?> ObtenerSegunId(PreceboId id, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<Precebo>().FirstOrDefaultAsync(x => x.Id == id)!;
    }

    public void AgregarCeba(Ceba entity)
    {
        DbContext.Add(entity);
    }

    public async Task<Ceba?> ObtenerSegunId(CebaId id, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<Ceba>().FirstOrDefaultAsync(x => x.Id == id)!;
    }

    public void AgregarSalidaCeba(SalidaCeba entity)
    {
        DbContext.Add(entity);
    }
}
