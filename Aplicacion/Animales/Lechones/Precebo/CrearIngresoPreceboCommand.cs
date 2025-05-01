using Aplicacion.Abstractions.Messaging;

namespace Aplicacion.Animales.Lechones.Precebo;

public record class CrearIngresoPreceboCommand(Guid DesteteId, DateTime FechaIngreso, decimal PesoPromedio, string Comentario) : ICommand;