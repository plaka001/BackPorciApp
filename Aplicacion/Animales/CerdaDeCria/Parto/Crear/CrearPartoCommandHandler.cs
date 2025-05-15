using Application.Abstractions.Messaging;
using Dominio.Abstractions;
using Dominio.Animales;
using Dominio.Animales.General;
using Dominio.Animales.ObjectValues;
using Dominio.Animales.Repository;

namespace Aplicacion.Animales.CerdaDeCria.Parto.Crear;

public sealed class CrearPartoCommandHandler : ICommandHandler<CrearPartoCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAnimalesRepository _animalesRepository;

    public CrearPartoCommandHandler(IUnitOfWork unitOfWork, IAnimalesRepository animalesRepository)
    {
        _unitOfWork = unitOfWork;
        _animalesRepository = animalesRepository;
    }

    public async Task<Result> Handle(CrearPartoCommand request, CancellationToken cancellationToken)
    {
        var cerdaExistente = await _animalesRepository.ObtenerSegunId(new CerdaCriaId( request.CerdaId));
        if (cerdaExistente == null)
            return Result.Failure(CerdaCriaErrores.NoEncontrada);

        if (cerdaExistente.EstadoProductivo != EstadoProductivo.Gestante)
            return Result.Failure(CerdaCriaErrores.ErrorEstadoProductivoCerdaGestante);

        cerdaExistente.CambiarEstado(EstadoProductivo.Lactante);
        cerdaExistente.RegistrarParto();
        var parto = Dominio.Animales.Parto.Create(new CerdaCriaId( request.CerdaId),request.FechaDeParto,request.CantidadVivos,request.CantidadMuertos,request.PesoPromedioVivos,request.PesoPromedioMuertos,request.UsoOxitocina,request.Comentario);
        _animalesRepository.Actualizar(cerdaExistente);
        _animalesRepository.AgregarParto(parto);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}
