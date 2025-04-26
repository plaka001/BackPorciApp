using Dominio.Abstractions;

namespace Dominio.Granjas;

public class GranjaErrores
{
    public static Error GranjaExistente = new(
     "Granja.Existente",
     "Ya existe la granja con este nombre"
    );

    public static Error NoEncontrada = new(
        "Granja.NoEncontrada",
        "La granja no existe"
        );
}
