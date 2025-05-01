using Dominio.Abstractions;

namespace Dominio.Animales.Lechones;


public class DesteteErrores
{
    public static Error PartoExistente = new(
     "Parto.Existente",
     "Ya el parto"
    );

    public static Error NoEncontrado = new(
        "Destete.NoEncontrado",
        "El destete no se ha encontrado"
        );
}