using Aplicacion.Abstractions.Messaging;
using Dominio.Animales.General;

namespace Aplicacion.Animales.CerdaDeCria.Trasladar;

public record class TrasladarCerdaCriaCommand(Guid EspacioFisicoOld,Guid EspacioFisicoNew, string IdentificacionCerda, EstadoProductivo EstadoProductivo) : ICommand;

