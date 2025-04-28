using Aplicacion.EspacioFisico.Calcular;
using Aplicacion.EspacioFisico.Crear;
using Dominio.granjas.ObjectValues;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EspaciosFisicosController : Controller
{
    private readonly ISender _sender;

    public EspaciosFisicosController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("Agregar")]
    public async Task<IActionResult> CrearEspacioFisico([FromBody] CrearEspacioFisicoRequest request, CancellationToken cancellationToken)
    {
        var granjaId = new GranjaId(request.GranjaId);
        var command = new CrearEspacioFisicoCommand(request.GranjaId, request.TipoEspacio, request.CantidadEspacios, request.CapacidadPorEspacio);
        var result = await _sender.Send(command);
        if (result.IsFailure) return BadRequest(result.Error);
        return Ok(result);
    }

    [HttpGet("ObtenerEspaciosFisicos")]
    public async Task<IActionResult> ObtenerEspaciosFisicos(Guid granjaId)
    {
        var query = new ListarEspaciosFisicosQuery(granjaId);
        var result = await _sender.Send(query);
        if (result.IsFailure) return BadRequest(result.Error);
        return Ok(result);

    }
}
