using Application.Abstractions.Messaging;
using Dominio.Abstractions;
using Dominio.granjas.ObjectValues;
using Dominio.Salud;
using Dominio.Salud.Repository;

namespace Aplicacion.Salud.PlanesSanitarios.Crear;

public sealed class CrearPlanSanitarioCommandHandler : ICommandHandler<CrearPlanSanitarioCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPlanSanitarioRepository _planSanitarioRepository;

    public CrearPlanSanitarioCommandHandler(IPlanSanitarioRepository planSanitarioRepository, IUnitOfWork unitOfWork)
    {
        _planSanitarioRepository = planSanitarioRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(CrearPlanSanitarioCommand request, CancellationToken cancellationToken)
    {
        var planSanitarioOld = await _planSanitarioRepository.ObtenerSegunNombre(request.Nombre, cancellationToken);
        if (planSanitarioOld != null) return Result.Failure(PlanSanitarioErrores.PlanSanitarioExistente);

        if (request.Eventos.Count <= 0) return Result.Failure(PlanSanitarioErrores.DebeAlMenosTenerUnEvento);

        var planSanitario = PlanSanitario.Create(new GranjaId(request.GranjaId), request.Nombre, request.TipoAplicacion);
        

        foreach (var evento in request.Eventos)
        {
            planSanitario.AgregarEventoPlan(evento.DiaDesdeAsignacion, evento.NombreEvento, evento.Comentario!);
        }

        _planSanitarioRepository.Agregar(planSanitario);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
