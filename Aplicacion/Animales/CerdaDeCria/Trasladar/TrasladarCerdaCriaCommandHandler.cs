using Application.Abstractions.Messaging;
using Dominio.Abstractions;
using Dominio.Animales;
using Dominio.Animales.General;
using Dominio.Animales.Repository;
using Dominio.EspacioFisicos;
using Dominio.EspacioFisicos.ObjectValues;
using Dominio.EspacioFisicos.Repository;
using Dominio.EspaciosFisico;

namespace Aplicacion.Animales.CerdaDeCria.Trasladar;

public sealed class TrasladarCerdaCriaCommandHandler : ICommandHandler<TrasladarCerdaCriaCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAnimalesRepository _animalesRepository;
    private readonly IEspacioFisicoRepository _espacioFisicosRepository;

    public TrasladarCerdaCriaCommandHandler(IUnitOfWork unitOfWork, IAnimalesRepository animalesRepository, IEspacioFisicoRepository espacioFisicosRepository)
    {
        _unitOfWork = unitOfWork;
        _animalesRepository = animalesRepository;
        _espacioFisicosRepository = espacioFisicosRepository;
    }

    public async Task<Result> Handle(TrasladarCerdaCriaCommand request, CancellationToken cancellationToken)
    {
        var cerdaExistente = await _animalesRepository.ObtenerCerdaByIdentificacion(request.IdentificacionCerda);
        if (cerdaExistente == null)
            return Result.Failure(CerdaCriaErrores.NoEncontrada);

        var espacioFisicoOld = await _espacioFisicosRepository.ObtenerSegunId(new EspacioFisicoId(request.EspacioFisicoOld));
        var espacioFisicoNew = await _espacioFisicosRepository.ObtenerSegunId(new EspacioFisicoId(request.EspacioFisicoNew));
        if (espacioFisicoNew == null || espacioFisicoOld == null)
            return Result.Failure(EspacioFisicoErrores.EspacioFisicoNoExistente);


        if (!espacioFisicoNew.EsTipoCorrectoParaEstadoProductivo(request.EstadoProductivo))
            return Result.Failure(EspacioFisicoErrores.TipoEspacioIncorrecto);

        if (!espacioFisicoNew.TieneCapacidadDisponible())
            return Result.Failure(EspacioFisicoErrores.SinCapacidad);

        espacioFisicoNew.IncrementarCapacidadOcupada(1);
        espacioFisicoOld.DecrementarCapacidadOcupada(1);
        _espacioFisicosRepository.Actualizar(espacioFisicoOld);
        _espacioFisicosRepository.Actualizar(espacioFisicoNew);

        cerdaExistente.Trasladar(new EspacioFisicoId(request.EspacioFisicoNew), DateTime.Now, request.EstadoProductivo);

        _animalesRepository.Actualizar(cerdaExistente);
        await _unitOfWork.SaveChangesAsync();
        return Result.Success();
    }
}
