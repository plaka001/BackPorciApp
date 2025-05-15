using Dominio.Abstractions;

namespace Dominio.Animales;


public class CerdaCriaErrores
{
    public static Error CerdaExistente = new(
     "Cerda.Existente",
     "Ya existe una cerda con esa identificacion"
    );

    public static Error NoEncontrada = new(
        "Cerda.NoEncontrada",
        "La cerda no existe"
        );

    public static Error ErrorEstadoProductivoCerdaGestante = new(
        "Cerda.EstadoErroneo",
        "Solo una cerda gestante puede parir"
        );

    public static Error ErrorEstadoProductivoTrasladoMonta = new(
      "Cerda.EstadoErroneo",
      "Para que la cerda sea montada debe estar en estado ingreso o vacia"
      );
}