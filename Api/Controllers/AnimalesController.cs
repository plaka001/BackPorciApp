using Aplicacion.Animales.CerdaDeCria.Crear;
using Aplicacion.Animales.CerdaDeCria.Parto.Crear;
using Aplicacion.Animales.CerdaDeCria.Trasladar;
using Aplicacion.Animales.Lechones.Destete;
using Aplicacion.Animales.Lechones.Precebo;
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

    [HttpPost("TrasladarCerdaCria")]
    public async Task<IActionResult> TrasladarCerdaCria([FromBody] TrasladarCerdaCriaRequest request, CancellationToken cancellationToken)
    {
        var command = new TrasladarCerdaCriaCommand(request.EspacioFisicoOld, request.EspacioFisicoNew,request.IdentificacionCerda,request.EstadoProductivo);
        var result = await _sender.Send(command);
        if (result.IsFailure) return BadRequest(result.Error);
        return Ok(result);
    }

    [HttpPost("RegistrarParto")]
    public async Task<IActionResult> RegistrarParto([FromBody] CrearPartoRequest request, CancellationToken cancellationToken)
    {
        var command = new CrearPartoCommand(request.CerdaId,request.CantidadVivos,request.CantidadMuertos,request.PesoPromedioVivos,request.PesoPromedioMuertos,request.UsoOxitocina,request.Comentario,request.FechaDeParto);
        var result = await _sender.Send(command);
        if (result.IsFailure) return BadRequest(result.Error);
        return Ok(result);
    }

    [HttpPost("RegistrarDestete")]
    public async Task<IActionResult> RegistrarDestete([FromBody] RegistrarDesteteRequest request, CancellationToken cancellationToken)
    {
        var command = new RegistrarDesteteCommand(request.PartoId,request.FechaDestete,request.CantidadVivos,request.CantidadMuertos,request.PesoPromedio,request.Comentario);
        var result = await _sender.Send(command);
        if (result.IsFailure) return BadRequest(result.Error);
        return Ok(result);
    }



    [HttpPost("RegistrarPrecebo")]
    public async Task<IActionResult> RegistrarPrecebo([FromBody] CrearIngresoPreceboRequest request, CancellationToken cancellationToken)
    {
        var command = new CrearIngresoPreceboCommand(request.DesteteId,request.FechaIngreso,request.PesoPromedio,request.Comentario);
        var result = await _sender.Send(command);
        if (result.IsFailure) return BadRequest(result.Error);
        return Ok(result);
    }
}
