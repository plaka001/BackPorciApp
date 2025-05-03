using Dominio.Abstractions;


namespace Dominio.EspaciosFisico;

public class EspacioFisicoErrores
{
    public static Error EspacioFisicoNoExistente = new(
     "EspacioFisico.NoExistente",
     "No existe el espacio fisico"
    );

    public static Error SinCapacidad = new(
    "EspacioFisico.SinCapacidad",
    "Sin espacio disponible para el espacio seleccionado"
   );

    public static Error TipoEspacioIncorrecto = new(
      "EspacioFisico.TipoEspacioIncorrecto",
      "El tipo de espacio no es correcto con la etapa de la cerda"
     );


    public static Error TipoEspacioIncorrectoOSinCapacidad = new(
     "EspacioFisico.TipoEspacioIncorrecto",
     "El tipo de espacio no es correcto con la etapa  o no hay capacidad"
    );

}
