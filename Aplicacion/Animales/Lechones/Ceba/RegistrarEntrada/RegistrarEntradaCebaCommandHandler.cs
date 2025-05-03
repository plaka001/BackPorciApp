using Aplicacion.Animales.Lechones.Ceba.RegistrarEntrada;
using Application.Abstractions.Messaging;
using Dominio.Abstractions;
using Dominio.Animales.Lechones;
using Dominio.Animales.ObjectValues;
using Dominio.Animales.Repository;
using Dominio.EspacioFisicos.ObjectValues;
using Dominio.EspacioFisicos.Repository;
using Dominio.EspaciosFisico;

public class RegistrarEntradaCebaCommandHandler : ICommandHandler<RegistrarEntradaCebaCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAnimalesRepository _animalesRepository;
    private readonly IEspacioFisicoRepository _espacioFisicoRepository;

    public RegistrarEntradaCebaCommandHandler(
        IUnitOfWork unitOfWork,
        IAnimalesRepository animalesRepository,
        IEspacioFisicoRepository espacioFisicoRepository)
    {
        _unitOfWork = unitOfWork;
        _animalesRepository = animalesRepository;
        _espacioFisicoRepository = espacioFisicoRepository;
    }

    public async Task<Result> Handle(RegistrarEntradaCebaCommand request, CancellationToken cancellationToken)
    {
        
        var precebo = await _animalesRepository.ObtenerSegunId(new PreceboId(request.PreceboId), cancellationToken);
        if (precebo == null)
            return Result.Failure(PreceboErrores.NoEncontrado);

        var cantidadInicialPrecebo = precebo.CantidadInicial;
        if (request.CantidadInicial > cantidadInicialPrecebo)
            return Result.Failure(CebaErrores.CebaErrorCantidad);

        var cantidadMuertosEnPrecebo = cantidadInicialPrecebo - request.CantidadInicial;

        precebo.RegistrarSalida(
            pesoPromedioFinal: request.PesoPromedioInicial,
            fechaSalida: request.FechaIngreso,
             cantidadMuertos: cantidadMuertosEnPrecebo);


        var espacioAntiguo = await _espacioFisicoRepository.ObtenerSegunId(precebo.EspacioFisicoId, cancellationToken);
        var espacioCeba = await _espacioFisicoRepository.ObtenerSegunTipo(TipoEspacio.Ceba, cancellationToken);

        if (espacioAntiguo == null || espacioCeba == null)
            return Result.Failure(EspacioFisicoErrores.TipoEspacioIncorrectoOSinCapacidad);


        espacioAntiguo.DecrementarCapacidadOcupada(cantidadInicialPrecebo);
        espacioCeba.IncrementarCapacidadOcupada(request.CantidadInicial);

        _espacioFisicoRepository.Actualizar(espacioAntiguo);
        _espacioFisicoRepository.Actualizar(espacioCeba);


        var ceba = Ceba.Create(
            preceboId: new PreceboId(request.PreceboId),
            fechaIngreso: request.FechaIngreso,
            cantidadInicial: request.CantidadInicial,
            pesoPromedioInicial: request.PesoPromedioInicial,
            espacioFisicoId: espacioCeba.Id!,
            comentario: request.Comentario);

        _animalesRepository.AgregarCeba(ceba);

        // 8. Persistir transacción
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
