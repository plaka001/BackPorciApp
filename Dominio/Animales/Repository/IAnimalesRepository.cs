namespace Dominio.Animales.Repository;

public interface IAnimalesRepository
{
    Task<CerdaCria?> ObtenerCerdaByIdentificacion(string identificacion);

    public void AgregarCerdaCria(CerdaCria entity);

    public void Actualizar(CerdaCria entity);


}
