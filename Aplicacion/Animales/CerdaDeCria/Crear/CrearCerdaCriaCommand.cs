using Aplicacion.Abstractions.Messaging;
using Dominio.Animales.General;
using Dominio.EspacioFisicos.ObjectValues;

namespace Aplicacion.Animales.CerdaDeCria.Crear;


public record class CrearCerdaCriaCommand(Guid GranjaId, string Identificacion, EstadoProductivo EstadoProductivo, DateTime FechaIngreso, Guid EspacioFisicoId, int NumeroDeParto, Guid PlanSanitarioId) : ICommand;