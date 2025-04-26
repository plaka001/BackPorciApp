using Aplicacion.Granjas.CrearGranja;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GranjaController : Controller
{
    private readonly ISender _sender;

    public GranjaController(ISender sender)
    {
        _sender = sender;
    }


    [HttpPost("Agregar")]
    public async Task<IActionResult> CrearGranja([FromBody] CrearGranjaRequest crearGranjaRequest, CancellationToken cancellationToken)
    {
        var command = new CrearGranjaCommand(crearGranjaRequest.Nombre,crearGranjaRequest.NumeroCerdasCria,crearGranjaRequest.Ubicacion,crearGranjaRequest.FechaInicioOperaciones);
        var result = await _sender.Send(command);
        if (result.IsFailure) return BadRequest(result.Error);
        return Ok(result);

    }
}
