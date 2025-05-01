using FluentValidation;

namespace Aplicacion.Animales.Lechones.Destete
{
    public class RegistrarDesteteValidator : AbstractValidator<RegistrarDesteteRequest>
    {
        public RegistrarDesteteValidator()
        {
            RuleFor(x => x.PartoId)
                .NotEmpty().WithMessage("El ID del parto es requerido");

            RuleFor(x => x.FechaDestete)
                .NotEmpty().WithMessage("La fecha de destete es requerida")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("La fecha de destete no puede ser futura");

            RuleFor(x => x.CantidadVivos)
                .GreaterThan(0).WithMessage("Debe haber al menos 1 lechón vivo destetado")
                .LessThanOrEqualTo(20).WithMessage("La cantidad de lechones vivos destetados no puede ser mayor a 20");

            RuleFor(x => x.CantidadMuertos)
                .GreaterThanOrEqualTo(0).WithMessage("La mortalidad no puede ser negativa")
                .LessThanOrEqualTo(20).WithMessage("La cantidad de lechones muertos no puede ser mayor a 20");

            RuleFor(x => x.PesoPromedio)
                .GreaterThan(0).WithMessage("El peso promedio debe ser mayor a 0")
                .LessThanOrEqualTo(15).WithMessage("El peso promedio no puede ser mayor a 15 kg");

            RuleFor(x => x.Comentario)
                .MaximumLength(500).WithMessage("El comentario no puede exceder los 500 caracteres");
        }
    }
}