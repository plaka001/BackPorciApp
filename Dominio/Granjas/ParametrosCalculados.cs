using Domain.Granjas;
using Dominio.Abstractions;
using Dominio.granjas.ObjectValues;
using Dominio.Granjas.ObjectValues;

namespace Dominio.Granjas;

public class ParametrosCalculados : Entity<ParametrosCalculadosId>
{
    // Propiedades (mantenemos las existentes)
    public GranjaId GranjaId { get; private set; }
    public int NumeroGrupos { get; private set; }
    public decimal CerdasPorGrupo { get; private set; }
    public int EspaciosParideras { get; private set; }
    public int EspaciosMontas { get; private set; }
    public int EspaciosGestacion { get; private set; }
    public int EspaciosPrecebo { get; private set; }
    public int EspaciosCeba { get; private set; }
    public DateTime FechaCalculo { get; private set; }

    // Constructor privado
    private ParametrosCalculados(
        ParametrosCalculadosId id,
        GranjaId granjaId,
        int numeroGrupos,
        decimal cerdasPorGrupo,
        int espaciosParideras,
        int espaciosMontas,
        int espaciosGestacion,
        int espaciosPrecebo,
        int espaciosCeba,
        DateTime fechaCalculo) : base(id)
    {
        GranjaId = granjaId;
        NumeroGrupos = numeroGrupos;
        CerdasPorGrupo = cerdasPorGrupo;
        EspaciosParideras = espaciosParideras;
        EspaciosMontas = espaciosMontas;
        EspaciosGestacion = espaciosGestacion;
        EspaciosPrecebo = espaciosPrecebo;
        EspaciosCeba = espaciosCeba;
        FechaCalculo = fechaCalculo;

    }

    // Factory Method con cálculos integrados
    public static ParametrosCalculados CalcularEspaciosInfraestructura(
        Granja granja,
        ParametrosProduccion parametros)
    {
        // Realizamos todos los cálculos
        var numeroGrupos = CalcularNumeroGrupos(parametros);
        var cerdasPorGrupo = CalcularCerdasPorGrupo(granja, numeroGrupos);
        var espaciosParideras = CalcularEspaciosParideras(parametros, cerdasPorGrupo);
        var espaciosMontas = CalcularEspaciosMontas(parametros, cerdasPorGrupo);
        var espaciosGestacion = CalcularEspaciosGestacion(parametros, cerdasPorGrupo);
        var espaciosPrecebo = CalcularEspaciosPrecebo(parametros, cerdasPorGrupo);
        var espaciosCeba = CalcularEspaciosCeba(parametros, cerdasPorGrupo);

        return new ParametrosCalculados(
            new ParametrosCalculadosId(Guid.NewGuid()),
            granja.Id!,
            numeroGrupos,
            cerdasPorGrupo,
            espaciosParideras,
            espaciosMontas,
            espaciosGestacion,
            espaciosPrecebo,
            espaciosCeba,
            DateTime.UtcNow);
    }

    #region Métodos de Cálculo
    private static int CalcularNumeroGrupos(ParametrosProduccion parametros)
    {
        int ritmo = parametros.RitmoProductivo > 0 ? parametros.RitmoProductivo : 7;
        int cicloProductivo = parametros.DuracionGestacion + parametros.DuracionLactancia;
        return (int)Math.Ceiling((decimal)cicloProductivo / ritmo);
    }


    private static decimal CalcularCerdasPorGrupo(Granja granja, int numeroGrupos)
    {
        if (numeroGrupos == 0) return 0;
        return (decimal)granja.NumeroCerdasCria / numeroGrupos;
    }


    private static int CalcularEspaciosParideras(ParametrosProduccion parametros, decimal cerdasPorGrupo)
    {
        int ritmo = parametros.RitmoProductivo > 0 ? parametros.RitmoProductivo : 7;
        decimal factor = (parametros.DuracionLactancia + parametros.TiempoAseoDescanso) / (decimal)ritmo;
        return (int)Math.Ceiling(cerdasPorGrupo * factor);
    }


    private static int CalcularEspaciosMontas(ParametrosProduccion parametros, decimal cerdasPorGrupo)
    {
        int ritmo = parametros.RitmoProductivo > 0 ? parametros.RitmoProductivo : 7;
        decimal factor = (parametros.IntervaloDesteteServicio + 5) / (decimal)ritmo;
        return (int)Math.Ceiling(cerdasPorGrupo * factor * 0.3m); // Se mantiene el 30% como estimación base
    }


    private static int CalcularEspaciosGestacion(ParametrosProduccion parametros, decimal cerdasPorGrupo)
    {
        int ritmo = parametros.RitmoProductivo > 0 ? parametros.RitmoProductivo : 7;
        int diasGestacionEfectiva = parametros.DuracionGestacion - parametros.ConfirmacionPrenez;
        decimal factor = diasGestacionEfectiva / (decimal)ritmo;
        return (int)Math.Ceiling(cerdasPorGrupo * factor);
    }


    private static int CalcularEspaciosPrecebo(ParametrosProduccion parametros, decimal cerdasPorGrupo)
    {
        decimal nacidosVivos = cerdasPorGrupo * parametros.PromedioNacidosVivos;
        decimal factorTiempo = parametros.DuracionPrecebo / 365m;
        decimal supervivencia = 1 - (parametros.MortalidadPrecebo / 100m);

        return (int)Math.Ceiling(nacidosVivos * factorTiempo * supervivencia);
    }



    private static int CalcularEspaciosCeba(ParametrosProduccion parametros, decimal cerdasPorGrupo)
    {
        decimal nacidosVivos = cerdasPorGrupo * parametros.PromedioNacidosVivos;
        decimal factorTiempo = parametros.DuracionCeba / 365m;
        decimal supervivencia = 1 - (parametros.MortalidadCeba  / 100m);
        return (int)Math.Ceiling(nacidosVivos * factorTiempo * supervivencia);
    }

    #endregion

    // Método para actualizar cálculos si los parámetros cambian
    public void Recalcular(Granja granja, ParametrosProduccion parametros)
    {
        NumeroGrupos = CalcularNumeroGrupos(parametros);
        CerdasPorGrupo = CalcularCerdasPorGrupo(granja, NumeroGrupos);
        EspaciosParideras = CalcularEspaciosParideras(parametros, CerdasPorGrupo);
        EspaciosMontas = CalcularEspaciosMontas(parametros, CerdasPorGrupo);
        EspaciosGestacion = CalcularEspaciosGestacion(parametros, CerdasPorGrupo);
        EspaciosPrecebo = CalcularEspaciosPrecebo(parametros, CerdasPorGrupo);
        EspaciosCeba = CalcularEspaciosCeba(parametros, CerdasPorGrupo);
        FechaCalculo = DateTime.UtcNow;
    }
}
