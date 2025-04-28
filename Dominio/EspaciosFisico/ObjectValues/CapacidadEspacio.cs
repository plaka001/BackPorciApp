namespace Dominio.EspacioFisicos.ObjectValues;

public sealed class CapacidadEspacio 
{
    public int CapacidadMaxima { get; private set; }
    public int CapacidadOcupada { get; private set; }

    private CapacidadEspacio(int capacidadMaxima, int capacidadOcupada)
    {
        if (capacidadMaxima <= 0)
            throw new ArgumentException("La capacidad máxima debe ser mayor que cero.");

        if (capacidadOcupada < 0 || capacidadOcupada > capacidadMaxima)
            throw new ArgumentException("La capacidad ocupada no puede ser menor que cero ni mayor que la capacidad máxima.");

        CapacidadMaxima = capacidadMaxima;
        CapacidadOcupada = capacidadOcupada;
    }

    public static CapacidadEspacio Create(int capacidadMaxima, int capacidadOcupada = 0)
    {
        return new CapacidadEspacio(capacidadMaxima, capacidadOcupada);
    }

    public bool TieneCapacidadDisponible(int cantidadAAgregar = 1)
    {
        return (CapacidadOcupada + cantidadAAgregar) <= CapacidadMaxima;
    }

    public CapacidadEspacio Incrementar(int cantidad)
    {
        if (!TieneCapacidadDisponible(cantidad))
            throw new InvalidOperationException("No hay suficiente capacidad disponible.");

        return new CapacidadEspacio(CapacidadMaxima, CapacidadOcupada + cantidad);
    }

    public CapacidadEspacio Decrementar(int cantidad)
    {
        if (CapacidadOcupada - cantidad < 0)
            throw new InvalidOperationException("No se puede decrementar por debajo de cero.");

        return new CapacidadEspacio(CapacidadMaxima, CapacidadOcupada - cantidad);
    }
}
