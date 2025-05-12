using Dominio.Abstractions;

namespace Dominio.Salud;



public class EventoSanitarioProgramadoErrores
{
    public static Error EntidadNoExiste = new(
     "Entidad.NoExistente",
     "La entidad seleccionada no existe"
    );

}
