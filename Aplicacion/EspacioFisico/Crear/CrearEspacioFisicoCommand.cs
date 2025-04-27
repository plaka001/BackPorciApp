using Aplicacion.Abstractions.Messaging;
using Dominio.EspacioFisicos.ObjectValues;

namespace Aplicacion.EspacioFisico.Crear;


public record class CrearEspacioFisicoCommand(Guid GranjaId, TipoEspacio TipoEspacio, int CantidadEspacios,int CapacidadPorEspacio ) : ICommand;
