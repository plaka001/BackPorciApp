using Dominio.EspacioFisicos.ObjectValues;

namespace Aplicacion.Animales.Lechones.Destete;


public record RegistrarDesteteRequest(Guid PartoId, DateTime FechaDestete, int CantidadVivos, int CantidadMuertos, decimal PesoPromedio,string Comentario);