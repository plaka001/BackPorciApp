using Application.Abstractions.Messaging;
using Dominio.Abstractions;
using Dominio.Animales;
using Dominio.Animales.CerdasCria;
using Dominio.Animales.General;
using Dominio.Animales.Repository;
using Dominio.EspacioFisicos.ObjectValues;
using Dominio.EspacioFisicos.Repository;
using Dominio.EspaciosFisico;

namespace Aplicacion.Animales.CerdaDeCria.Prenez;

public class ConfirmarPrenezCerdaCommandHandler : ICommandHandler<ConfirmarPrenezCerdaCommand>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IAnimalesRepository _animalesRepository;
    private readonly IEspacioFisicoRepository _espacioFisicoRepository;

    public ConfirmarPrenezCerdaCommandHandler(IAnimalesRepository animalesRepository, IUnitOfWork unitOfWork, IEspacioFisicoRepository espacioFisicoRepository)
    {
        _animalesRepository = animalesRepository;
        _unitOfWork = unitOfWork;
        _espacioFisicoRepository = espacioFisicoRepository;
    }

    public async Task<Result> Handle(ConfirmarPrenezCerdaCommand request, CancellationToken cancellationToken)
    {
        var cerdaCria = await _animalesRepository.ObtenerCerdaByIdentificacion(request.IdentificacionCerda);
        if (cerdaCria == null) return Result.Failure(CerdaCriaErrores.NoEncontrada);

        if (cerdaCria.EstadoProductivo != EstadoProductivo.Servida) return Result.Failure(ConfirmarPrenezCerdaErrores.CerdaExistente);

        if (request.EstaPreñada)
        {
            var espacioFisicoNew = await _espacioFisicoRepository.ObtenerSegunTipo(TipoEspacio.Gestacion, cancellationToken);
            var espacioFisicoOld = await _espacioFisicoRepository.ObtenerSegunId(cerdaCria.EspacioFisicoId, cancellationToken);
            if (espacioFisicoNew == null || espacioFisicoOld == null) return Result.Failure(EspacioFisicoErrores.EspacioFisicoNoExistente);

            if (!espacioFisicoNew.TieneCapacidadDisponible()) return Result.Failure(EspacioFisicoErrores.SinCapacidad);
            cerdaCria.Trasladar(espacioFisicoNew.Id!, DateTime.Now, EstadoProductivo.Gestante);

            espacioFisicoNew.IncrementarCapacidadOcupada(1);
            espacioFisicoOld.DecrementarCapacidadOcupada(1);
            _espacioFisicoRepository.Actualizar(espacioFisicoNew);
            _espacioFisicoRepository.Actualizar(espacioFisicoOld);
        }
        else
        {
            cerdaCria.CambiarEstado(EstadoProductivo.Vacia);
        }

        _animalesRepository.Actualizar(cerdaCria);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();

    }
}
