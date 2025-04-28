using Dominio.Animales.ObjectValues;

namespace Dominio.Animales.Repository;

public interface IAnimalesRepository
{
    Task<CerdaCria> ObtenerSegunId(
       CerdaCriaId id,
       CancellationToken cancellationToken = default
   );
    Task<CerdaCria?> ObtenerCerdaByIdentificacion(string identificacion);

    public void AgregarCerdaCria(CerdaCria entity);

    public void Actualizar(CerdaCria entity);

    public void AgregarParto(Parto entity);


}
