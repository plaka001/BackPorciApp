using Dominio.EspacioFisicos.ObjectValues;
namespace Dominio.EspacioFisicos.Repository;

public interface IEspacioFisicoRepository
{
    public void Agregar(EspacioFisico entity);

    Task<EspacioFisico?> ObtenerSegunId(EspacioFisicoId id,CancellationToken cancellationToken = default);

    Task<EspacioFisico?> ObtenerSegunTipo( TipoEspacio tipoEspacio, CancellationToken cancellationToken = default);
    public void Actualizar(EspacioFisico entity);
}
