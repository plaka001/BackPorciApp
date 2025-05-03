using Aplicacion.Abstractions.Messaging;

namespace Aplicacion.Animales.Lechones.Ceba.RegistrarEntrada;

public record class RegistrarEntradaCebaCommand(Guid PreceboId, DateTime FechaIngreso,int CantidadInicial, decimal PesoPromedioInicial, string Comentario) : ICommand;