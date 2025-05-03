using Dominio.Abstractions;
using Dominio.Animales.ObjectValues;

namespace Dominio.Animales.Lechones;

public class SalidaCeba : Entity<SalidaCebaId>
{
    public CebaId CebaId { get; private set; }
    public DateTime FechaSalida { get; private set; }
    public decimal PesoPromedioFinal { get; private set; }
    public int CantidadMuertos { get; private set; }
    public int CantidadVivos { get; private set; }
    public DateTime FechaCreacion { get; private set; }

    private SalidaCeba(
        SalidaCebaId id,
        CebaId cebaId,
        DateTime fechaSalida,
        decimal pesoPromedioFinal,
        int cantidadMuertos,
        int cantidadVivos) : base(id)
    {
        CebaId = cebaId;
        FechaSalida = fechaSalida;
        PesoPromedioFinal = pesoPromedioFinal;
        CantidadMuertos = cantidadMuertos;
        CantidadVivos = cantidadVivos;
        FechaCreacion = DateTime.UtcNow;
    }

    public static SalidaCeba Create(
        CebaId cebaId,
        DateTime fechaSalida,
        decimal pesoPromedioFinal,
        int cantidadMuertos,
        int cantidadVivos)
    {
        // Validaciones
        if (cebaId == null)
            throw new ArgumentException("El ID de ceba es requerido");

        if (fechaSalida > DateTime.UtcNow)
            throw new ArgumentException("La fecha de salida no puede ser futura");

        if (pesoPromedioFinal <= 0)
            throw new ArgumentException("El peso promedio final debe ser mayor a 0");

        if (cantidadMuertos < 0)
            throw new ArgumentException("La cantidad de muertos no puede ser negativa");

        if (cantidadVivos <= 0)
            throw new ArgumentException("Debe haber al menos 1 animal vivo");

        return new SalidaCeba(
            SalidaCebaId.New(),
            cebaId,
            fechaSalida,
            pesoPromedioFinal,
            cantidadMuertos,
            cantidadVivos);
    }
}