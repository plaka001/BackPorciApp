namespace Aplicacion.Animales.Lechones.Precebo;

public record CrearIngresoPreceboRequest(Guid DesteteId, DateTime FechaIngreso, decimal PesoPromedio, string Comentario);