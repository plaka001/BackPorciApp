namespace Dominio.Animales.Repository;

public interface IAnimalesRepository
{
    Task<CerdaCria?> ObtenerCerdaByIdentificacion(string identificacion);

    public void AgregarCerdaCria(CerdaCria entity);
    
}
