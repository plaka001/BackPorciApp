using FluentValidation;

namespace Aplicacion.Animales.Lechones.Ceba.RegistrarSalida;

public class RegistrarSalidaCebaCommandValidator : AbstractValidator<RegistrarSalidaCebaRequest>
{
    public RegistrarSalidaCebaCommandValidator()
    {
        RuleFor(x => x.CebaId)
            .NotEmpty().WithMessage("El ID de la ceba es requerido");

        RuleFor(x => x.FechaSalida)
            .NotEmpty().WithMessage("La fecha de salida es requerida")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("La fecha de salida no puede ser futura");

        RuleFor(x => x.PesoPromedioFinal)
            .GreaterThan(0).WithMessage("El peso promedio final debe ser mayor a 0")
            .LessThanOrEqualTo(150).WithMessage("El peso promedio final no puede ser mayor a 150 kg");

        RuleFor(x => x.CantidadVivos)
            .GreaterThanOrEqualTo(0).WithMessage("La cantidad de animales vivos no puede ser negativa")
            .LessThanOrEqualTo(1000).WithMessage("La cantidad de animales vivos no puede ser mayor a 1000");

        RuleFor(x => x.CantidadMuertos)
            .GreaterThanOrEqualTo(0).WithMessage("La cantidad de animales muertos no puede ser negativa")
            .LessThanOrEqualTo(1000).WithMessage("La cantidad de animales muertos no puede ser mayor a 1000")
            .Must((request, muertos) => request.CantidadVivos + muertos > 0)
            .WithMessage("Debe haber al menos un animal (vivo o muerto)");
    }
}

