using Dominio.Abstractions;

namespace Dominio.Salud;


public class EventoSanitarioPlanErrores
{
    public static Error SinEventos = new(
     "Eventos.Sin",
     "No hay eventos para ese plan programadoe"
    );
}
