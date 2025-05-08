using Dominio.Abstractions;

namespace Dominio.Salud;


public class PlanSanitarioErrores
{
    public static Error PlanSanitarioExistente = new(
     "PlanSanitario.Existente",
     "Ya existe el plan  con este nombre"
    );


    public static Error DebeAlMenosTenerUnEvento = new(
     "PlanSanitario.SinEventos",
     "Debe al menos tener un evento registrado "
    );
}
