﻿using Aplicacion.Animales.CerdaDeCria.Crear;
using Aplicacion.Animales.CerdaDeCria.Parto.Crear;
using Aplicacion.Animales.CerdaDeCria.Prenez;
using Aplicacion.Animales.CerdaDeCria.Servicio.Registrar;
using Aplicacion.Animales.CerdaDeCria.Trasladar;
using Aplicacion.Animales.Lechones.Ceba.RegistrarEntrada;
using Aplicacion.Animales.Lechones.Ceba.RegistrarSalida;
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


    [HttpPost("RegitrarServicioCerdaCria")]
    public async Task<IActionResult> RegitrarServicioCerdaCria([FromBody] RegistrarServicioCerdaRequest request, CancellationToken cancellationToken)
    {
        var command = new RegistrarServicioCerdaCommand(request.IdentificacionCerda,request.FechaServicio);
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


    [HttpPost("RegistrarPrenezCerdaCria")]
    public async Task<IActionResult> RegistrarPrenezCerdaCria([FromBody] ConfirmarPrenezCerdaRequest request, CancellationToken cancellationToken)
    {
        var command = new ConfirmarPrenezCerdaCommand(request.IdentificacionCerda,request.FechaConfirmacion,request.EstaPreñada);
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
        var command = new CrearIngresoPreceboCommand(request.DesteteId,request.EspacioFisicoId, request.FechaIngreso,request.PesoPromedio,request.Comentario);
        var result = await _sender.Send(command);
        if (result.IsFailure) return BadRequest(result.Error);
        return Ok(result);
    }

    [HttpPost("RegistrarCeba")]
    public async Task<IActionResult> RegistrarCeba([FromBody] RegistrarEntradaCebaRequest request, CancellationToken cancellationToken)
    {
        var command = new RegistrarEntradaCebaCommand(request.PreceboId,request.FechaIngreso,request.CantidadInicial,request.PesoPromedioInicial,request.Comentario);
        var result = await _sender.Send(command);
        if (result.IsFailure) return BadRequest(result.Error);
        return Ok(result);
    }

    [HttpPost("RegistrarSalidaCeba")]
    public async Task<IActionResult> RegistrarSalidaCeba([FromBody] RegistrarSalidaCebaRequest request, CancellationToken cancellationToken)
    {
        var command = new RegistrarSalidaCebaCommand(request.CebaId,request.FechaSalida,request.PesoPromedioFinal,request.CantidadVivos,request.CantidadMuertos);
        var result = await _sender.Send(command);
        if (result.IsFailure) return BadRequest(result.Error);
        return Ok(result);
    }
}
