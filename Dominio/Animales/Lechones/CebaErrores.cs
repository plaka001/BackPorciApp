using Dominio.Abstractions;

namespace Dominio.Animales.Lechones;

public class CebaErrores
{
    public static Error CebaErrorCantidad = new(
     "Ceba.Cantidad",
     "LA Cantidad a ingrsar a ceba es mayor a la cantidad inicial del lote precebo"
    );

    public static Error CebaNoEncontrada = new(
    "Ceba.NoEncontrada",
    "El numero de ceba no se encontro en la base de datos"
   );
}
