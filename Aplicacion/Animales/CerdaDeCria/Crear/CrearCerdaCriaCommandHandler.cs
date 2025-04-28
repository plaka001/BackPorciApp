using Application.Abstractions.Messaging;
using Dominio.Abstractions;
using Dominio.Animales;
using Dominio.Animales.Repository;
using Dominio.EspacioFisicos.ObjectValues;
using Dominio.EspacioFisicos.Repository;
using Dominio.EspaciosFisico;
using Dominio.granjas.ObjectValues;

namespace Aplicacion.Animales.CerdaDeCria.Crear;

public sealed class CrearCerdaCriaCommandHandler : ICommandHandler<CrearCerdaCriaCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAnimalesRepository _animalesRepository;
    private readonly IEspacioFisicoRepository _espacioFisicosRepository;

    public CrearCerdaCriaCommandHandler(
        IAnimalesRepository animalesRepository,
        IEspacioFisicoRepository espacioFisicosRepository,
        IUnitOfWork unitOfWork)
    {
        _animalesRepository = animalesRepository;
        _espacioFisicosRepository = espacioFisicosRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(CrearCerdaCriaCommand request, CancellationToken cancellationToken)
    {
        var cerdaExistente = await _animalesRepository.ObtenerCerdaByIdentificacion(request.Identificacion);
        if (cerdaExistente != null)
            return Result.Failure(CerdaCriaErrores.CerdaExistente);

        var espacioFisico = await _espacioFisicosRepository.ObtenerSegunId(new EspacioFisicoId(request.EspacioFisicoId));
        if (espacioFisico == null)
            return Result.Failure(EspacioFisicoErrores.EspacioFisicoNoExistente);

        if (!espacioFisico.EsTipoCorrectoParaEstadoProductivo(request.EstadoProductivo))
            return Result.Failure(EspacioFisicoErrores.TipoEspacioIncorrecto);

        if (!espacioFisico.TieneCapacidadDisponible())
            return Result.Failure(EspacioFisicoErrores.SinCapacidad);

        espacioFisico.IncrementarCapacidadOcupada(1);
        _espacioFisicosRepository.Actualizar(espacioFisico);

        var cerda = CerdaCria.Create(
            new GranjaId(request.GranjaId),
            request.Identificacion,
            request.EstadoProductivo,
            DateTime.Now,
            new EspacioFisicoId(request.EspacioFisicoId),
            request.NumeroDeParto,
            request.PlanSanitarioId);

        _animalesRepository.AgregarCerdaCria(cerda);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
