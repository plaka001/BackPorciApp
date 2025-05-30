using Aplicacion.Abstractions.Messaging;

namespace Aplicacion.Animales.CerdaDeCria.Prenez;

public sealed record ConfirmarPrenezCerdaCommand(string IdentificacionCerda, DateTime FechaConfirmacion, bool EstaPreñada) : ICommand;