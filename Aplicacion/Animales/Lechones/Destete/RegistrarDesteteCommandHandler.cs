using Application.Abstractions.Messaging;
using Dominio.Abstractions;
using Dominio.Animales;
using Dominio.Animales.General;
using Dominio.Animales.ObjectValues;
using Dominio.Animales.Repository;
using Dominio.EspacioFisicos.ObjectValues;
using Dominio.EspacioFisicos.Repository;
using Dominio.EspaciosFisico;

namespace Aplicacion.Animales.Lechones.Destete;

public class RegistrarDesteteCommandHandler : ICommandHandler<RegistrarDesteteCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAnimalesRepository _animalesRepository;
    private readonly IEspacioFisicoRepository _espacioFisicoRepository;
    public RegistrarDesteteCommandHandler(IUnitOfWork unitOfWork, IAnimalesRepository animalesRepository, IEspacioFisicoRepository espacioFisicoRepository)
    {
        _unitOfWork = unitOfWork;
        _animalesRepository = animalesRepository;
        _espacioFisicoRepository = espacioFisicoRepository;
    }

    public async Task<Result> Handle(RegistrarDesteteCommand request, CancellationToken cancellationToken)
    {
        var parto = await _animalesRepository.ObtenerSegunId(new PartoId(request.PartoId));
        if (parto == null) return Result.Failure(PartoErrores.NoEncontrada);

        var cerdaCria = await _animalesRepository.ObtenerSegunId(parto.CerdaCriaId);
        if (cerdaCria == null) return Result.Failure(CerdaCriaErrores.NoEncontrada);
        if (cerdaCria.EstadoProductivo != EstadoProductivo.Lactante)
            return Result.Failure(CerdaCriaErrores.EstadoProductivoNoLactante);


        var espacioFisicoActual = await _espacioFisicoRepository.ObtenerSegunId(cerdaCria.EspacioFisicoId);
        var espacioFisicoMonta = await _espacioFisicoRepository.ObtenerSegunTipo(TipoEspacio.Monta);

        if (espacioFisicoActual == null || espacioFisicoMonta == null) return Result.Failure(EspacioFisicoErrores.TipoEspacioIncorrectoOSinCapacidad);
        if (!espacioFisicoMonta.TieneCapacidadDisponible())
            return Result.Failure(EspacioFisicoErrores.SinCapacidad);


        var destete = Dominio.Animales.Lechones.Destete.Create(new PartoId(request.PartoId), request.FechaDestete, request.CantidadVivos, request.CantidadMuertos, request.PesoPromedio, request.Comentario);

        cerdaCria.Destetar(espacioFisicoMonta.Id!, request.FechaDestete);
        espacioFisicoActual.DecrementarCapacidadOcupada(1);
        espacioFisicoMonta.IncrementarCapacidadOcupada(1);
        _animalesRepository.Actualizar(cerdaCria);
        _espacioFisicoRepository.Actualizar(espacioFisicoMonta);
        _espacioFisicoRepository.Actualizar(espacioFisicoActual);
        _animalesRepository.AgregarDestete(destete);
        await _unitOfWork.SaveChangesAsync();
        return Result.Success();

    }
}
