using Domain.Granjas;
using Dominio.Granjas;

namespace Dominio.granjas.repository;

public interface IGranjaRepository
{
    public void Agregar(Granja entity);
    public void AgregarParametrosProduccion(ParametrosProduccion entity);
    public void AgregarParametrosProduccionCalculados(ParametrosCalculados entity);

    public  Task<Granja?> ObtenerGranjaByNombre(string Nombre);
}
