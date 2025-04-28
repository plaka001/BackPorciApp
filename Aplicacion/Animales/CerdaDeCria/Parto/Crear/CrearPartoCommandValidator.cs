using FluentValidation;

namespace Aplicacion.Animales.CerdaDeCria.Parto.Crear;

public class RegistrarPartoCommandValidator : AbstractValidator<CrearPartoCommand>
{
    public RegistrarPartoCommandValidator()
    {
        RuleFor(c => c.CerdaId)
            .NotEmpty().WithMessage("El ID de la cerda es requerido");

        RuleFor(c => c.CantidadVivos)
            .GreaterThanOrEqualTo(0).WithMessage("La cantidad de vivos no puede ser negativa");

        RuleFor(c => c.CantidadMuertos)
            .GreaterThanOrEqualTo(0).WithMessage("La cantidad de muertos no puede ser negativa");

        RuleFor(c => c.PesoPromedioVivos)
            .GreaterThan(0).WithMessage("El peso promedio de vivos debe ser positivo");

        RuleFor(c => c.PesoPromedioMuertos)
            .GreaterThanOrEqualTo(0).WithMessage("El peso promedio de muertos no puede ser negativo");
    }
}