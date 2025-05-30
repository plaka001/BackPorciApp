using Aplicacion.Abstractions.Messaging;

namespace Aplicacion.Animales.Lechones.Precebo;

public record class CrearIngresoPreceboCommand(Guid DesteteId, Guid EspacioFisicoId, DateTime FechaIngreso, decimal PesoPromedio, string Comentario) : ICommand;