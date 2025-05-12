using Aplicacion.Animales.CerdaDeCria.Trasladar;
using Aplicacion.Salud.PlanesSanitarios.Asignar;
using Aplicacion.Salud.PlanesSanitarios.Crear;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;


[Route("api/[controller]")]
[ApiController]
public class SaludController: ControllerBase
{
    private readonly ISender _sender;

    public SaludController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("CrearPlanSanitario")]
    public async Task<IActionResult> CrearPlanSanitario([FromBody] CrearPlanSanitarioRequest request, CancellationToken cancellationToken)
    {
        var command = new CrearPlanSanitarioCommand(request.GranjaId,request.Nombre,request.TipoAplicacion,request.Eventos);
        var result = await _sender.Send(command);
        if (result.IsFailure) return BadRequest(result.Error);
        return Ok(result);
    }

    [HttpPost("AsignarPlanSanitario")]
    public async Task<IActionResult> AsignarPlanSanitario([FromBody] AsignarPlanSanitarioRequest request, CancellationToken cancellationToken)
    {
        var command = new AsignarPlanSanitarioCommand(request.PlanSanitarioId,request.TipoEntidadId,request.EntidadId,request.FechaAsignacion);
        var result = await _sender.Send(command);
        if (result.IsFailure) return BadRequest(result.Error);
        return Ok(result);
    }
}
