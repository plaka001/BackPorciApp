using Dominio.Animales.General;

namespace Aplicacion.Animales.CerdaDeCria.Crear;

public record CrearCerdaCriaRequest(Guid GranjaId, string Identificacion, EstadoProductivo EstadoProductivo, DateTime FechaIngreso,Guid EspacioFisicoId,int NumeroDeParto,Guid PlanSanitarioId);



