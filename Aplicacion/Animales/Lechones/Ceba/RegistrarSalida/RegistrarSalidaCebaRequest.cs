namespace Aplicacion.Animales.Lechones.Ceba.RegistrarSalida;


public record RegistrarSalidaCebaRequest(Guid CebaId, DateTime FechaSalida, decimal PesoPromedioFinal, int CantidadVivos, int CantidadMuertos);