using FluentValidation;

namespace Aplicacion.Salud.PlanesSanitarios.Crear;

public class CrearPlanSanitarioCommandValidator : AbstractValidator<CrearPlanSanitarioCommand>
{
    public CrearPlanSanitarioCommandValidator()
    {
        RuleFor(x => x.GranjaId)
            .NotEmpty().WithMessage("El ID de la granja es requerido");

        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre del plan sanitario es requerido")
            .MaximumLength(200).WithMessage("El nombre no puede exceder los 200 caracteres");

        RuleFor(x => x.TipoAplicacion)
            .IsInEnum().WithMessage("Tipo de aplicación no válido");

        RuleFor(x => x.Eventos)
            .NotEmpty().WithMessage("Debe incluir al menos un evento sanitario");

        RuleForEach(x => x.Eventos)
            .ChildRules(evento =>
            {
                evento.RuleFor(e => e.DiaDesdeAsignacion)
                    .GreaterThanOrEqualTo(0).WithMessage("El día desde asignación no puede ser negativo");

                evento.RuleFor(e => e.NombreEvento)
                    .NotEmpty().WithMessage("El nombre del evento es requerido")
                    .MaximumLength(200).WithMessage("El nombre del evento no puede exceder los 200 caracteres");

                evento.RuleFor(e => e.Comentario)
                    .MaximumLength(500).WithMessage("El comentario no puede exceder los 500 caracteres");
            });
    }
}

