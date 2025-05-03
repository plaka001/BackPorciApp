using FluentValidation;

namespace Aplicacion.Animales.Lechones.Ceba.RegistrarEntrada;

internal class RegistrarEntradaCebaCommandValidator : AbstractValidator<RegistrarEntradaCebaRequest>
{
    public RegistrarEntradaCebaCommandValidator()
    {
        RuleFor(x => x.PreceboId)
            .NotEmpty().WithMessage("El ID del precebo es requerido");

        RuleFor(x => x.FechaIngreso)
            .NotEmpty().WithMessage("La fecha de ingreso es requerida")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("La fecha de ingreso no puede ser futura");

        RuleFor(x => x.PesoPromedioInicial)
            .GreaterThan(0).WithMessage("El peso promedio inicial debe ser mayor a 0")
            .LessThanOrEqualTo(50).WithMessage("El peso promedio inicial no puede ser mayor a 50 kg");

        RuleFor(x => x.CantidadInicial)
         .GreaterThan(0).WithMessage("El cantidad inicial inicial debe ser mayor a 0");    

        RuleFor(x => x.Comentario)
            .MaximumLength(500).WithMessage("El comentario no puede exceder los 500 caracteres");
    }
}
