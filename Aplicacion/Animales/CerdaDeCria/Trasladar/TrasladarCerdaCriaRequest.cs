using Dominio.Animales;

namespace Aplicacion.Animales.CerdaDeCria.Trasladar;

public record TrasladarCerdaCriaRequest(Guid EspacioFisicoOld,Guid EspacioFisicoNew, string IdentificacionCerda, EstadoProductivo EstadoProductivo);

