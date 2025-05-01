using Aplicacion.Animales.Lechones.Precebo;
using FluentValidation;

namespace Aplicacion.Animales.Precebo
{
    public class CrearIngresoPreceboValidator : AbstractValidator<CrearIngresoPreceboRequest>
    {
        public CrearIngresoPreceboValidator()
        {
            RuleFor(x => x.DesteteId)
                .NotEmpty().WithMessage("El ID del destete es requerido");

            RuleFor(x => x.FechaIngreso)
                .NotEmpty().WithMessage("La fecha de ingreso es requerida")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("La fecha de ingreso no puede ser futura");

            RuleFor(x => x.PesoPromedio)
                .GreaterThan(0).WithMessage("El peso promedio debe ser mayor a 0")
                .LessThanOrEqualTo(30).WithMessage("El peso promedio no puede ser mayor a 30 kg");

            RuleFor(x => x.Comentario)
                .MaximumLength(500).WithMessage("El comentario no puede exceder los 500 caracteres");
        }
    }
}