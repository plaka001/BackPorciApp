

using Dominio.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructura.Repositorios;

internal abstract class Repository<TEntity, TEntityId>
where TEntity : Entity<TEntityId>
where TEntityId : class
{
    protected readonly ApplicationDbContext DbContext;

    protected Repository(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<TEntity?> ObtenerSegunId(
        TEntityId id,
        CancellationToken cancellationToken = default
    )
    {
        return await DbContext.Set<TEntity>()
        .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public void Agregar(TEntity entity)
    {
        DbContext.Add(entity);
    }

    public void Actualizar(TEntity entity)
    {
        DbContext.Update(entity);
    }
}