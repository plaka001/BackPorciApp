using Aplicacion.Abstractions.Messaging;
using Aplicacion.Dtos;
using Dominio.Salud.Enums;

namespace Aplicacion.Salud.PlanesSanitarios.Crear;

public record class CrearPlanSanitarioCommand(
    Guid GranjaId,
    string Nombre,
    TipoAplicacionSanitaria TipoAplicacion, // "Cerda" o "Lote"
    List<EventoSanitarioDto> Eventos
) : ICommand;

