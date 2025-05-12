using Application.Abstractions.Messaging;
using Dominio.Abstractions;
using Dominio.Animales.ObjectValues;
using Dominio.Animales.Repository;
using Dominio.Salud;
using Dominio.Salud.Enums;
using Dominio.Salud.ObjectValues;
using Dominio.Salud.Repository;
using System.Numerics;

namespace Aplicacion.Salud.PlanesSanitarios.Asignar;

public sealed class AsignarPlanSanitarioCommandHandler : ICommandHandler<AsignarPlanSanitarioCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPlanSanitarioRepository _planSanitarioRepository;
    private readonly IAnimalesRepository _animalesRepository;

    public AsignarPlanSanitarioCommandHandler(IUnitOfWork unitOfWork, IPlanSanitarioRepository planSanitarioRepository, IAnimalesRepository animalesRepository)
    {
        _unitOfWork = unitOfWork;
        _planSanitarioRepository = planSanitarioRepository;
        _animalesRepository = animalesRepository;
    }

    public async Task<Result> Handle(AsignarPlanSanitarioCommand request, CancellationToken cancellationToken)
    {
        var planSanitario = await _planSanitarioRepository.ObtenerPorIdConEventosAsync(new PlanSanitarioId(request.PlanSanitarioId), cancellationToken);
        if (planSanitario == null) return Result.Failure(PlanSanitarioErrores.PlanSanitarioNoExistente);

        var entidad = await ExisteAsync( (TipoAplicacionSanitaria)request.TipoEntidadId, request.EntidadId,cancellationToken);
        if (entidad == null) return Result.Failure(EventoSanitarioProgramadoErrores.EntidadNoExiste);
        

        foreach (var eventoPlan in planSanitario.EventosPlan)
        {
            var fechaProgramada = request.FechaAsignacion.AddDays(eventoPlan.DiaDesdeAsignacion);

            planSanitario.AgregarEvento(
                request.TipoEntidadId,
                request.EntidadId,
                eventoPlan.NombreEvento,
                fechaProgramada,
                eventoPlan.Comentario
            );
        }

        _planSanitarioRepository.Agregar(planSanitario.Eventos);
        await _unitOfWork.SaveChangesAsync();
        return Result.Success(planSanitario);
    }

    public async Task<dynamic?> ExisteAsync(TipoAplicacionSanitaria tipo, Guid entidadId, CancellationToken ct)
    {
        return tipo switch
        {
            TipoAplicacionSanitaria.Cerda => await _animalesRepository.ObtenerSegunId(new CerdaCriaId(entidadId),ct),
            TipoAplicacionSanitaria.LotePrecebo => await _animalesRepository.ObtenerSegunId(new PreceboId(entidadId), ct),
            TipoAplicacionSanitaria.LoteCeba => await _animalesRepository.ObtenerSegunId(new CebaId(entidadId), ct),
            _ => false
        };
    }

}
