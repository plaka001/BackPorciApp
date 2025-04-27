using FluentValidation;

namespace Aplicacion.Granjas.CrearGranja
{
    public class CrearGranjaCommandValidator : AbstractValidator<CrearGranjaCommand>
    {
        public CrearGranjaCommandValidator()
        {
            RuleFor(c => c.Nombre)
                .NotEmpty().WithMessage("El nombre de la granja es requerido")
                .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres");

            RuleFor(c => c.NumeroCerdasCria)
                .GreaterThan(0).WithMessage("Debe tener al menos 1 cerda de cría");

            RuleFor(c => c.Ubicacion)
                .MaximumLength(200).WithMessage("La ubicación no puede exceder 200 caracteres")
                .When(c => !string.IsNullOrEmpty(c.Ubicacion));
        }
    }
}