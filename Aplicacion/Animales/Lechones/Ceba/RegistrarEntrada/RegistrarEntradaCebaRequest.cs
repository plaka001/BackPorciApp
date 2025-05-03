namespace Aplicacion.Animales.Lechones.Ceba.RegistrarEntrada;

public record RegistrarEntradaCebaRequest(Guid PreceboId, DateTime FechaIngreso,int CantidadInicial, decimal PesoPromedioInicial, string Comentario);