using Dominio.Abstractions;
using Dominio.Animales.General;
using Dominio.EspacioFisicos.ObjectValues;
using Dominio.granjas.ObjectValues;

namespace Dominio.EspacioFisicos;

public class EspacioFisico : Entity<EspacioFisicoId>
{
    // Propiedades
    public GranjaId GranjaId { get; private set; }
    public string TipoEspacio { get; private set; }
    public int CantidadEspacios { get; private set; }
    public int CapacidadPorEspacio { get; private set; }
    public int CapacidadRecomendada { get; private set; }
    public CapacidadEspacio Capacidad { get; private set; }
    public DateTime FechaCreacion { get; private set; }

    // Constructor privado modificado
    protected EspacioFisico(
        EspacioFisicoId id,
        GranjaId granjaId,
        string tipoEspacio,
        int cantidadEspacios,
        int capacidadPorEspacio,
        int capacidadRecomendada,
        DateTime fechaCreacion) : base(id)
    {
        GranjaId = granjaId;
        TipoEspacio = tipoEspacio;
        CantidadEspacios = cantidadEspacios;
        CapacidadPorEspacio = capacidadPorEspacio;
        CapacidadRecomendada = capacidadRecomendada;
        FechaCreacion = fechaCreacion;

        // Inicialización de Capacidad después de la construcción
        Capacidad = CapacidadEspacio.Create(cantidadEspacios * capacidadPorEspacio);
    }

    // Factory method
    public static EspacioFisico Create(
        GranjaId granjaId,
        string tipoEspacio,
        int cantidadEspacios,
        int capacidadPorEspacio = 1,
        int capacidadRecomendada = 0)
    {

        return new EspacioFisico(
            EspacioFisicoId.New(),
            granjaId,
            tipoEspacio,
            cantidadEspacios,
            capacidadPorEspacio,
            capacidadRecomendada,
            DateTime.UtcNow);
    }

    // Métodos de negocio
    public void ActualizarCapacidades(int cantidadEspacios, int capacidadPorEspacio)
    {
        if (cantidadEspacios <= 0 || capacidadPorEspacio <= 0)
            throw new ArgumentException("Los valores deben ser mayores a cero");

        CantidadEspacios = cantidadEspacios;
        CapacidadPorEspacio = capacidadPorEspacio;
        Capacidad = CapacidadEspacio.Create(cantidadEspacios * capacidadPorEspacio, Capacidad.CapacidadOcupada);
    }

    public void IncrementarCapacidadOcupada(int cantidad)
    {
        Capacidad = Capacidad.Incrementar(cantidad);
    }

    public void DecrementarCapacidadOcupada(int cantidad)
    {
        Capacidad = Capacidad.Decrementar(cantidad);
    }

    public virtual bool TieneCapacidadDisponible(int cantidadAAgregar = 1)
    {
        return Capacidad.TieneCapacidadDisponible(cantidadAAgregar);
    }

    public virtual bool EsTipoCorrectoParaEstadoProductivo(EstadoProductivo estadoProductivo)
    {
        return estadoProductivo switch
        {
            EstadoProductivo.Ingreso => TipoEspacio.Equals("Ingreso", StringComparison.OrdinalIgnoreCase),
            EstadoProductivo.Servida => TipoEspacio.Equals("Monta", StringComparison.OrdinalIgnoreCase),
            EstadoProductivo.Gestante => TipoEspacio.Equals("Gestacion", StringComparison.OrdinalIgnoreCase),
            EstadoProductivo.Lactante => TipoEspacio.Equals("Paridera", StringComparison.OrdinalIgnoreCase),
            EstadoProductivo.Vacia => TipoEspacio.Equals("Monta", StringComparison.OrdinalIgnoreCase),
            EstadoProductivo.Reformada or EstadoProductivo.Muerta => true, 
        };
    }

}