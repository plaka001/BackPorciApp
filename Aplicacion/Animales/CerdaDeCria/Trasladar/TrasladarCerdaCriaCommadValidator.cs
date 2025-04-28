using FluentValidation;

namespace Aplicacion.Animales.CerdaDeCria.Trasladar;

public class TrasladarCerdaCriaCommandValidator : AbstractValidator<TrasladarCerdaCriaCommand>
{
    public TrasladarCerdaCriaCommandValidator()
    {
        RuleFor(c => c.EspacioFisicoOld)
            .NotEmpty().WithMessage("El ID del espacio físico origen es requerido.")
            .NotEqual(Guid.Empty).WithMessage("El ID del espacio físico origen no es válido.");

        RuleFor(c => c.EspacioFisicoNew)
            .NotEmpty().WithMessage("El ID del espacio físico destino es requerido.")
            .NotEqual(Guid.Empty).WithMessage("El ID del espacio físico destino no es válido.")
            .NotEqual(c => c.EspacioFisicoOld).WithMessage("El espacio destino no puede ser igual al origen.");

        RuleFor(c => c.IdentificacionCerda)
            .NotEmpty().WithMessage("La identificación de la cerda es requerida.")
            .MaximumLength(50).WithMessage("La identificación no puede exceder 50 caracteres.");

        RuleFor(c => c.EstadoProductivo)
            .IsInEnum().WithMessage("El estado productivo proporcionado no es válido.")
            .WithMessage("El estado productivo no puede ser 'Desconocido'.");
    }
}
