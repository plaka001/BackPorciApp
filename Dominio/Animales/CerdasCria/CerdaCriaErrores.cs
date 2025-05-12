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

    public static Error ErrorEstadoProductivo = new(
        "Cerda.EstadoErroneo",
        "Solo una cerda gestante puede parir"
        );
}