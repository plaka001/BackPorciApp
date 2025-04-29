using Dominio.Animales;

namespace Aplicacion.Animales.CerdaDeCria.Parto.Crear;

public record CrearPartoRequest(Guid CerdaId, int CantidadVivos, int CantidadMuertos, decimal PesoPromedioVivos,decimal PesoPromedioMuertos,bool UsoOxitocina,string Comentario, DateTime FechaDeParto);
