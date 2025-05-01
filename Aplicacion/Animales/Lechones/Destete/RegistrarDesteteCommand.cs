

using Aplicacion.Abstractions.Messaging;

namespace Aplicacion.Animales.Lechones.Destete;

public record class RegistrarDesteteCommand(Guid PartoId, DateTime FechaDestete, int CantidadVivos, int CantidadMuertos, decimal PesoPromedio, string Comentario):ICommand;

