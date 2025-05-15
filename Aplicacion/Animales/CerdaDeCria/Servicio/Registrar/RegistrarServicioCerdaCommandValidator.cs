using FluentValidation;

namespace Aplicacion.Animales.CerdaDeCria.Servicio.Registrar;

public class RegistrarServicioCerdaCommandValidator : AbstractValidator<RegistrarServicioCerdaCommand>
{
    public RegistrarServicioCerdaCommandValidator()
    {
        RuleFor(x => x.IdentificacionCerda)
            .NotEmpty().WithMessage("La identificación de la cerda es obligatoria.")
            .MaximumLength(20).WithMessage("La identificación no puede exceder los 20 caracteres.");

        RuleFor(x => x.FechaServicio)
            .NotEmpty().WithMessage("La fecha de servicio es obligatoria.")
            .LessThanOrEqualTo(DateTime.Today).WithMessage("La fecha de servicio no puede ser en el futuro.");
    }
}
