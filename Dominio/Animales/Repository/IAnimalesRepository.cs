using Dominio.Animales.CerdasCria;
using Dominio.Animales.Lechones;
using Dominio.Animales.ObjectValues;

namespace Dominio.Animales.Repository;

public interface IAnimalesRepository
{
    Task<CerdaCria?> ObtenerSegunId(
       CerdaCriaId id,
       CancellationToken cancellationToken = default
   );
    Task<CerdaCria?> ObtenerCerdaByIdentificacion(string identificacion);

    public void AgregarCerdaCria(CerdaCria entity);

    public void Actualizar(CerdaCria entity);

    public void AgregarParto(Parto entity);
    public void AgregarCeba(Ceba entity);

    public void AgregarSalidaCeba(SalidaCeba entity);
    public void AgregarDestete(Destete entity);


    Task<Parto?> ObtenerSegunId(
     PartoId id,
     CancellationToken cancellationToken = default
 );

    Task<Destete?> ObtenerSegunId(
    DesteteId id,
    CancellationToken cancellationToken = default
);

    Task<Precebo?> ObtenerSegunId(
  PreceboId id,
  CancellationToken cancellationToken = default
);

    Task<Ceba?> ObtenerSegunId(
CebaId id,
CancellationToken cancellationToken = default
);
    void agregarPrecebo(Precebo entity);
}
