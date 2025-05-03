using Application.Abstractions.Messaging;
using Dominio.Abstractions;
using Dominio.Animales.Lechones;
using Dominio.Animales.ObjectValues;
using Dominio.Animales.Repository;
using Dominio.EspacioFisicos.Repository;
using Dominio.EspaciosFisico;

namespace Aplicacion.Animales.Lechones.Ceba.RegistrarSalida;

public class RegistrarSalidaCebaCommandHandler : ICommandHandler<RegistrarSalidaCebaCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEspacioFisicoRepository _espacioFisicosRepository;
    private readonly IAnimalesRepository _animalesRepository;

    public RegistrarSalidaCebaCommandHandler(IUnitOfWork unitOfWork, IEspacioFisicoRepository espacioFisicosRepository, IAnimalesRepository animalesRepository)
    {
        _unitOfWork = unitOfWork;
        _espacioFisicosRepository = espacioFisicosRepository;
        _animalesRepository = animalesRepository;
    }

    public async Task<Result> Handle(RegistrarSalidaCebaCommand request, CancellationToken cancellationToken)
    {
        var ceba = await _animalesRepository.ObtenerSegunId(new CebaId(request.CebaId));
        if (ceba == null) return Result.Failure(CebaErrores.CebaNoEncontrada);
        var salidaCeba = SalidaCeba.Create(new CebaId(request.CebaId), request.FechaSalida, request.PesoPromedioFinal, request.CantidadMuertos, request.CantidadVivos);
        ceba.RegistrarSalida(request.PesoPromedioFinal,request.FechaSalida,request.CantidadMuertos);

        var EspacioFisico = await _espacioFisicosRepository.ObtenerSegunId(ceba.EspacioFisicoId);
        if (EspacioFisico == null) return Result.Failure(EspacioFisicoErrores.EspacioFisicoNoExistente);
        var cantidadLiberar = request.CantidadMuertos + request.CantidadVivos;
        EspacioFisico.DecrementarCapacidadOcupada(cantidadLiberar);
        _animalesRepository.AgregarSalidaCeba(salidaCeba);
        await _unitOfWork.SaveChangesAsync();
        return Result.Success();
    }
}
