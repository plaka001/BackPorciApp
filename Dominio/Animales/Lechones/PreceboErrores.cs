using Dominio.Abstractions;

namespace Dominio.Animales.Lechones;

public class PreceboErrores
{
    public static Error NoEncontrado = new(
        "Precebo.NoEncontrado",
        "El Precebo no se ha encontrado"
        );
}