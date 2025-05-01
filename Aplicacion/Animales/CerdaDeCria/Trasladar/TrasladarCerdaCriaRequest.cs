using Dominio.Animales.General;

namespace Aplicacion.Animales.CerdaDeCria.Trasladar;

public record TrasladarCerdaCriaRequest(Guid EspacioFisicoOld,Guid EspacioFisicoNew, string IdentificacionCerda, EstadoProductivo EstadoProductivo);

