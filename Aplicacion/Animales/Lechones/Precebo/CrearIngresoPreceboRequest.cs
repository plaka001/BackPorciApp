namespace Aplicacion.Animales.Lechones.Precebo;

public record CrearIngresoPreceboRequest(Guid DesteteId, Guid EspacioFisicoId, DateTime FechaIngreso, decimal PesoPromedio, string Comentario);