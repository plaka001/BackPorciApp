using Aplicacion.Abstractions.Messaging;

namespace Aplicacion.Animales.Lechones.Ceba.RegistrarSalida;

public sealed record RegistrarSalidaCebaCommand(Guid CebaId, DateTime FechaSalida, decimal PesoPromedioFinal, int CantidadVivos, int CantidadMuertos) : ICommand;

