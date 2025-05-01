using Dominio.Abstractions;
using Dominio.Animales.ObjectValues;

namespace Dominio.Animales;

public class Parto : Entity<PartoId>
{
    public CerdaCriaId CerdaCriaId { get; private set; }
    public DateTime FechaParto { get; private set; }
    public int CantidadVivos { get; private set; }
    public int CantidadMuertos { get; private set; }
    public decimal PesoPromedioVivos { get; private set; }
    public decimal PesoPromedioMuertos { get; private set; }
    public bool UsoOxitocina { get; private set; }
    public string? Comentario { get; private set; }
    public DateTime FechaCreacion { get; private set; }

    private Parto(
        PartoId id,
        CerdaCriaId cerdaCriaId,
        DateTime fechaParto,
        int cantidadVivos,
        int cantidadMuertos,
        decimal pesoPromedioVivos,
        decimal pesoPromedioMuertos,
        bool usoOxitocina,
        string? comentario) : base(id)
    {
        CerdaCriaId = cerdaCriaId;
        FechaParto = fechaParto;
        CantidadVivos = cantidadVivos;
        CantidadMuertos = cantidadMuertos;
        PesoPromedioVivos = pesoPromedioVivos;
        PesoPromedioMuertos = pesoPromedioMuertos;
        UsoOxitocina = usoOxitocina;
        Comentario = comentario;
        FechaCreacion = DateTime.UtcNow;
    }

    public static Parto Create(
        CerdaCriaId cerdaCriaId,
        DateTime fechaParto,
        int cantidadVivos,
        int cantidadMuertos,
        decimal pesoPromedioVivos,
        decimal pesoPromedioMuertos,
        bool usoOxitocina,
        string? comentario = null)
    {
        // Validaciones
        if (fechaParto > DateTime.UtcNow)
            throw new ArgumentException("La fecha de parto no puede ser futura");

        if (cantidadVivos < 0 || cantidadMuertos < 0)
            throw new ArgumentException("Las cantidades no pueden ser negativas");

        return new Parto(
            PartoId.New(),
            cerdaCriaId,
            fechaParto,
            cantidadVivos,
            cantidadMuertos,
            pesoPromedioVivos,
            pesoPromedioMuertos,
            usoOxitocina,
            comentario);
    }

    public void ActualizarComentario(string nuevoComentario)
    {
        Comentario = nuevoComentario;
    }
}
