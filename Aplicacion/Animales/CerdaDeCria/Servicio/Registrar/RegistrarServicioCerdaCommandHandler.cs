using Application.Abstractions.Messaging;
using Dominio.Abstractions;
using Dominio.Animales;
using Dominio.Animales.General;
using Dominio.Animales.Repository;
using Dominio.EspacioFisicos.ObjectValues;
using Dominio.EspacioFisicos.Repository;
using Dominio.EspaciosFisico;

namespace Aplicacion.Animales.CerdaDeCria.Servicio.Registrar;

public class RegistrarServicioCerdaCommandHandler : ICommandHandler<RegistrarServicioCerdaCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAnimalesRepository _animalesRepository;
    private readonly IEspacioFisicoRepository _espacioFisicoRepository;

    public RegistrarServicioCerdaCommandHandler(IUnitOfWork unitOfWork, IAnimalesRepository animalesRepository, IEspacioFisicoRepository espacioFisicoRepository)
    {
        _unitOfWork = unitOfWork;
        _animalesRepository = animalesRepository;
        _espacioFisicoRepository = espacioFisicoRepository;
    }

    public async Task<Result> Handle(RegistrarServicioCerdaCommand request, CancellationToken cancellationToken)
    {
        var cerdaDeCria = await _animalesRepository.ObtenerCerdaByIdentificacion(request.IdentificacionCerda);
        if (cerdaDeCria == null) return Result.Failure(CerdaCriaErrores.NoEncontrada);

        var estadosValidos = new[] { EstadoProductivo.Ingreso, EstadoProductivo.Vacia };
        if (!estadosValidos.Contains(cerdaDeCria.EstadoProductivo))
            return Result.Failure(CerdaCriaErrores.ErrorEstadoProductivoTrasladoMonta);

        var espacioFisicoActual = await _espacioFisicoRepository.ObtenerSegunId(cerdaDeCria.EspacioFisicoId);
        if (espacioFisicoActual == null) return Result.Failure(EspacioFisicoErrores.EspacioFisicoNoExistente);

        if (espacioFisicoActual.TipoEspacio != TipoEspacio.Monta.ToString())
        {
            var espaciofisicoNuevo = await _espacioFisicoRepository.ObtenerSegunTipo(TipoEspacio.Monta, cancellationToken);
            if (espaciofisicoNuevo == null || !espaciofisicoNuevo.TieneCapacidadDisponible()) return Result.Failure(EspacioFisicoErrores.TipoEspacioIncorrectoOSinCapacidad);
            cerdaDeCria.Trasladar(espaciofisicoNuevo.Id!, request.FechaServicio, EstadoProductivo.Servida);
            espacioFisicoActual.DecrementarCapacidadOcupada(1);
            espaciofisicoNuevo.IncrementarCapacidadOcupada(1);
            _espacioFisicoRepository.Actualizar(espacioFisicoActual);
            _espacioFisicoRepository.Actualizar(espaciofisicoNuevo);
        }
        else
        {
            cerdaDeCria.CambiarEstado(EstadoProductivo.Servida);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();    
    }
}
