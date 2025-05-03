using Dominio.Abstractions;
using Dominio.granjas.ObjectValues;

namespace Domain.Granjas;

public class Granja : Entity<GranjaId>
{
    public string Nombre { get; private set; }
    public string Ubicacion { get; private set; }
    public int NumeroCerdasCria { get; private set; }
    public DateTime FechaCreacion { get; private set; }

    protected Granja(
        GranjaId id,
        string nombre,
        string ubicacion,
        int numeroCerdasCria,
        DateTime fechaCreacion) : base(id)
    {
        Nombre = nombre;
        Ubicacion = ubicacion;
        NumeroCerdasCria = numeroCerdasCria;
        FechaCreacion = fechaCreacion;
    }

    public static Granja Create(
        string nombre,
        string ubicacion,
        int numeroCerdasCria,
        DateTime? fechaCreacion = null)
    {
        return new Granja(
            new GranjaId(Guid.NewGuid()),
            nombre,
            ubicacion,
            numeroCerdasCria,
            fechaCreacion ?? DateTime.UtcNow);
    }
}