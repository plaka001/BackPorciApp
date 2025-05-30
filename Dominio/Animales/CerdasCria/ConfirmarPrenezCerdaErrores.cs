using Dominio.Abstractions;

namespace Dominio.Animales.CerdasCria;

public class ConfirmarPrenezCerdaErrores
{
    public static Error CerdaExistente = new(
     "Prenez.Estado",
     "Solo se puede registrar ele stado de prenez a una cerda que esta en estado servida"
    );
}
