using FluentValidation;

namespace Aplicacion.Salud.PlanesSanitarios.Asignar;


public class AsignarPlanSanitarioCommandValidator : AbstractValidator<AsignarPlanSanitarioCommand>
{
    public AsignarPlanSanitarioCommandValidator()
    {
        RuleFor(x => x.PlanSanitarioId)
            .NotEmpty().WithMessage("El ID del plan sanitario es requerido");

        RuleFor(x => x.TipoEntidadId)
            .InclusiveBetween(1, 3).WithMessage("El tipo de entidad debe ser 1 (Cerda), 2 (Precebo) o 3 (Ceba)");

        RuleFor(x => x.EntidadId)
            .NotEmpty().WithMessage("El ID de la entidad es requerido");

        RuleFor(x => x.FechaAsignacion)
            .NotEmpty().WithMessage("La fecha de asignación es requerida")
            .LessThanOrEqualTo(DateTime.Today.AddDays(1)).WithMessage("La fecha de asignación no puede ser futura");
    }
}