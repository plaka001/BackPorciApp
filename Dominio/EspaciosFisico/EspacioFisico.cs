using Dominio.Abstractions;
using Dominio.EspacioFisicos.ObjectValues;
using Dominio.granjas.ObjectValues;

namespace Dominio.EspacioFisicos;

public class EspacioFisico : Entity<EspacioFisicoId>
{

    public GranjaId GranjaId { get; private set; }
    public string TipoEspacio { get; private set; }
    public int CantidadEspacios { get; private set; }
    public int CapacidadPorEspacio { get; private set; }
    public int CapacidadRecomendada { get; private set; }
    public int CapacidadReal { get; private set; }
    public DateTime FechaCreacion { get; private set; }

    private EspacioFisico(
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
        CapacidadReal = cantidadEspacios * capacidadPorEspacio;
        FechaCreacion = fechaCreacion;
    }

    public static EspacioFisico Create(
        GranjaId granjaId,
        string tipoEspacio,
        int cantidadEspacios,
        int capacidadPorEspacio = 1,
        int capacidadRecomendada = 0)
    {

        return new EspacioFisico(
            new EspacioFisicoId(Guid.NewGuid()),
            granjaId,
            tipoEspacio,
            cantidadEspacios,
            capacidadPorEspacio,
            capacidadRecomendada,
            DateTime.UtcNow);
    }

    // Método para actualizar capacidades
    public void ActualizarCapacidades(int cantidadEspacios, int capacidadPorEspacio)
    {
        if (cantidadEspacios <= 0 || capacidadPorEspacio <= 0)
            throw new ArgumentException("Los valores deben ser mayores a cero");

        CantidadEspacios = cantidadEspacios;
        CapacidadPorEspacio = capacidadPorEspacio;
        CapacidadReal = cantidadEspacios * capacidadPorEspacio;
    }
}

