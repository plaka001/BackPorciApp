using Aplicacion.Abstractions.Messaging;

namespace Aplicacion.Granjas.CrearGranja;

public record class CrearGranjaCommand(string Nombre, int NumeroCerdasCria, string? Ubicacion, DateTime FechaInicioOperaciones) : ICommand;

