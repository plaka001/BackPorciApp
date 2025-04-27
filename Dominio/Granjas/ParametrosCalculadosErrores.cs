
using Dominio.Abstractions;

namespace Dominio.Granjas;

public class ParametrosCalculadosErrores
{
    public static Error ParametrosCalculadosNoExisten = new(
     "ParametrosCalculados.Existente",
     "Ya existe la granja con este nombre"
    );

}




