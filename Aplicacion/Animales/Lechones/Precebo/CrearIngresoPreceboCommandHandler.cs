using Application.Abstractions.Messaging;
using Dominio.Abstractions;
using Dominio.Animales.Lechones;
using Dominio.Animales.ObjectValues;
using Dominio.Animales.Repository;
using Dominio.EspacioFisicos.ObjectValues;
using Dominio.EspacioFisicos.Repository;
using Dominio.EspaciosFisico;

namespace Aplicacion.Animales.Lechones.Precebo;

public sealed class CrearIngresoPreceboCommandHandler : ICommandHandler<CrearIngresoPreceboCommand>
{
    private readonly IAnimalesRepository _animalesRepository;
    private readonly IEspacioFisicoRepository _espacioFisicoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CrearIngresoPreceboCommandHandler(IAnimalesRepository animalesRepository, IEspacioFisicoRepository espacioFisicoRepository, IUnitOfWork unitOfWork)
    {
        _animalesRepository = animalesRepository;
        _espacioFisicoRepository = espacioFisicoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(CrearIngresoPreceboCommand request, CancellationToken cancellationToken)
    {
        var destete = await _animalesRepository.ObtenerSegunId(new DesteteId(request.DesteteId), cancellationToken);
        if (destete == null) return Result.Failure(DesteteErrores.NoEncontrado);
        var espacioFisico = await _espacioFisicoRepository.ObtenerSegunTipo(TipoEspacio.Precebo);
        if (espacioFisico == null) return Result.Failure(EspacioFisicoErrores.TipoEspacioIncorrectoOSinCapacidad);
        espacioFisico.IncrementarCapacidadOcupada(destete.CantidadDestetados);
        var precebo = Dominio.Animales.Lechones.Precebo.Create(new DesteteId(request.DesteteId), request.FechaIngreso, destete.CantidadDestetados,request.PesoPromedio, espacioFisico.Id!,request.Comentario);
        _espacioFisicoRepository.Actualizar(espacioFisico);
        _animalesRepository.agregarPrecebo(precebo);
        await _unitOfWork.SaveChangesAsync();
        return Result.Success(precebo);
    }
}
