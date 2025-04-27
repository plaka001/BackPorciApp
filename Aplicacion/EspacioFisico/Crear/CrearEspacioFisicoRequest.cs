using Dominio.EspacioFisicos.ObjectValues;
using Dominio.granjas.ObjectValues;

namespace Aplicacion.EspacioFisico.Crear;

public record CrearEspacioFisicoRequest(Guid GranjaId, TipoEspacio TipoEspacio,int CantidadEspacios, int CapacidadPorEspacio);
