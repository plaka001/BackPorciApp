using Aplicacion.Abstractions.Messaging;

namespace Aplicacion.Animales.CerdaDeCria.Parto.Crear;

public record class CrearPartoCommand(Guid CerdaId, int CantidadVivos, int CantidadMuertos, decimal PesoPromedioVivos, decimal PesoPromedioMuertos, bool UsoOxitocina, string Comentario,DateTime FechaDeParto) : ICommand;

