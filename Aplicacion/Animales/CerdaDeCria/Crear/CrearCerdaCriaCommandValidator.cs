using FluentValidation;

namespace Aplicacion.Animales.CerdaDeCria.Crear;

public class CrearCerdaCriaCommandValidator : AbstractValidator<CrearCerdaCriaCommand>
{
    public CrearCerdaCriaCommandValidator()
    {
        RuleFor(c => c.GranjaId)
            .NotEmpty().WithMessage("El ID de la granja es obligatorio");

        RuleFor(c => c.Identificacion)
            .NotEmpty().WithMessage("La identificación es obligatoria")
            .MaximumLength(100).WithMessage("La identificación no puede exceder 100 caracteres");

        RuleFor(c => c.EstadoProductivo)
            .IsInEnum().WithMessage("El estado productivo no es válido");

        RuleFor(c => c.FechaIngreso)
            .NotEmpty().WithMessage("La fecha de ingreso es obligatoria")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("La fecha de ingreso no puede ser futura");

        RuleFor(c => c.EspacioFisicoId)
            .NotEmpty().WithMessage("El ID del espacio físico es obligatorio");

    }
}