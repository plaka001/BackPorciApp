using Dominio.Animales.General;
using Dominio.EspacioFisicos.ObjectValues;
using Dominio.EspacioFisicos;
using Dominio.granjas.ObjectValues;

namespace Unit.Test.Fakes;

public class FakeEspacioFisico : EspacioFisico
{
    // Propiedades para controlar el comportamiento en tests
    public bool TieneCapacidadDisponibleResult { get; set; }
    public bool EsTipoCorrectoResult { get; set; }

    // Constructor simplificado para tests
    public FakeEspacioFisico(
        string tipoEspacio = "Paridera",
        bool tieneCapacidad = true,
        bool esTipoCorrecto = true)
        : base(
            EspacioFisicoId.New(),
            GranjaId.New(),
            tipoEspacio,
            cantidadEspacios: 10,
            capacidadPorEspacio: 1,
            capacidadRecomendada: 0,
            fechaCreacion: DateTime.UtcNow)
    {
        TieneCapacidadDisponibleResult = tieneCapacidad;
        EsTipoCorrectoResult = esTipoCorrecto;
    }

    // Sobrescribimos los métodos para controlar su comportamiento
    public override bool TieneCapacidadDisponible(int cantidadAAgregar = 1)
        => TieneCapacidadDisponibleResult;

    public override bool EsTipoCorrectoParaEstadoProductivo(EstadoProductivo estadoProductivo)
        => EsTipoCorrectoResult;

    public static FakeEspacioFisico  DeTipo(string tipoEspacio, bool tieneCapacidad = true) => new(
        tieneCapacidad: tieneCapacidad,
        tipoEspacio : tipoEspacio
    );
}
