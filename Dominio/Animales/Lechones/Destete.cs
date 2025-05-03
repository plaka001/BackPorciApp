using Dominio.Abstractions;
using Dominio.Animales.ObjectValues;

namespace Dominio.Animales.Lechones;

public class Destete : Entity<DesteteId>
{
    public PartoId PartoId { get; private set; }
    public DateTime FechaDestete { get; private set; }
    public int CantidadDestetados { get; private set; }
    public int MortalidadPreDestete { get; private set; }
    public decimal PesoPromedioDestetados { get; private set; }
    public string Comentario { get; private set; }
    public DateTime FechaCreacion { get; private set; }

    protected Destete(
        DesteteId id,
        PartoId partoId,
        DateTime fechaDestete,
        int cantidadDestetados,
        int mortalidadPreDestete,
        decimal pesoPromedioDestetados,
        string comentario) : base(id)
    {
        PartoId = partoId;
        FechaDestete = fechaDestete;
        CantidadDestetados = cantidadDestetados;
        MortalidadPreDestete = mortalidadPreDestete;
        PesoPromedioDestetados = pesoPromedioDestetados;
        Comentario = comentario;
        FechaCreacion = DateTime.UtcNow;
    }

    public static Destete Create(
        PartoId partoId,
        DateTime fechaDestete,
        int cantidadDestetados,
        int mortalidadPreDestete,
        decimal pesoPromedioDestetados,
        string comentario = null)
    {
        // Validaciones
        if (partoId == null)
            throw new ArgumentException("El ID del parto es requerido");

        if (fechaDestete > DateTime.UtcNow)
            throw new ArgumentException("La fecha de destete no puede ser futura");

        if (cantidadDestetados <= 0)
            throw new ArgumentException("Debe haber al menos 1 lechón destetado");

        if (mortalidadPreDestete < 0)
            throw new ArgumentException("La mortalidad no puede ser negativa");

        if (pesoPromedioDestetados <= 0)
            throw new ArgumentException("El peso promedio debe ser mayor a 0");

        return new Destete(
            DesteteId.New(),
            partoId,
            fechaDestete,
            cantidadDestetados,
            mortalidadPreDestete,
            pesoPromedioDestetados,
            comentario);
    }
}