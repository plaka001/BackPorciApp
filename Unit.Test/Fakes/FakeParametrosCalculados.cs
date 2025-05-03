using Domain.Granjas;
using Dominio.granjas.ObjectValues;
using Dominio.Granjas.ObjectValues;
using Dominio.Granjas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unit.Test.Fakes;

public class FakeParametrosCalculados : ParametrosCalculados
{
    public bool WasRecalculated { get; private set; }

    public FakeParametrosCalculados(
        GranjaId granjaId = null,
        int numeroGrupos = 5,
        decimal cerdasPorGrupo = 20m,
        int espaciosParideras = 10,
        int espaciosMontas = 3,
        int espaciosGestacion = 15,
        int espaciosPrecebo = 50,
        int espaciosCeba = 100)
        : base(
            new ParametrosCalculadosId(Guid.NewGuid()),
            granjaId ?? GranjaId.New(),
            numeroGrupos,
            cerdasPorGrupo,
            espaciosParideras,
            espaciosMontas,
            espaciosGestacion,
            espaciosPrecebo,
            espaciosCeba,
            DateTime.UtcNow)
    {
    }

    public override void Recalcular(Granja granja, ParametrosProduccion parametros)
    {
        WasRecalculated = true;
        base.Recalcular(granja, parametros);
    }

    public static FakeParametrosCalculados CreateWithDefaults() => new();
}