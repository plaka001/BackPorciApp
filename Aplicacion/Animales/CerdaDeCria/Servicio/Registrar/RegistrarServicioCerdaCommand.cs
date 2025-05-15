using Aplicacion.Abstractions.Messaging;

namespace Aplicacion.Animales.CerdaDeCria.Servicio.Registrar;

public sealed record RegistrarServicioCerdaCommand(string IdentificacionCerda, DateTime FechaServicio) : ICommand;

