using Domain.Granjas;
using Dominio.granjas.ObjectValues;
using Dominio.Granjas;

namespace Dominio.granjas.repository;

public interface IGranjaRepository
{
    void Agregar(Granja entity);
    void AgregarParametrosProduccion(ParametrosProduccion entity);
    void AgregarParametrosProduccionCalculados(ParametrosCalculados entity);
    Task<Granja?> ObtenerGranjaByNombre(string Nombre);
    Task<Granja?> ObtenerSegunId(GranjaId id, CancellationToken cancellationToken);
    Task<ParametrosCalculados?> obtenerParametrsCalculados(GranjaId granjaId);
}
