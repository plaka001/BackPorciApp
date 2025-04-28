using Aplicacion.Animales.CerdaDeCria.Crear;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AnimalesController : ControllerBase
{
    private readonly ISender _sender;

    public AnimalesController(ISender sender)
    {
        _sender = sender;
    }


    [HttpPost("CrearCerdaCria")]
    public async Task<IActionResult> CrearCerdaCria([FromBody] CrearCerdaCriaRequest request, CancellationToken cancellationToken)
    {
        var command = new CrearCerdaCriaCommand(request.GranjaId,request.Identificacion, request.EstadoProductivo,request.FechaIngreso,request.EspacioFisicoId,request.NumeroDeParto,request.PlanSanitarioId);
        var result = await _sender.Send(command);
        if (result.IsFailure) return BadRequest(result.Error);
        return Ok(result);
    }
}
