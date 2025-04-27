using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.EspacioFisico.Crear;

public class CrearEspacioFisicoCommandValidator: AbstractValidator<CrearEspacioFisicoCommand>
{
    public CrearEspacioFisicoCommandValidator()
    {
        RuleFor(c => c.GranjaId)
                .NotEmpty().WithMessage("El Id de la granja es obligatorio");

        RuleFor(c => c.TipoEspacio)
            .IsInEnum().WithMessage("El tipo de espacio físico no es válido");

        RuleFor(c => c.CapacidadPorEspacio)
            .GreaterThan(0).WithMessage("La capacidad real debe ser mayor que cero");

        RuleFor(c => c.CapacidadPorEspacio)
            .GreaterThan(0).WithMessage("La capacidad por espacio debe ser mayor que cero");
    }
}
